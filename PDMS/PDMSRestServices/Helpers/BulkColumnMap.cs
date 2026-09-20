
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MES.Cred.Jobs.Helpers
{
    /// <summary>
    /// Attribute to map DTO properties to destination table columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BulkColumnAttribute : Attribute
    {
        public string? Name { get; }
        public int Order { get; set; } = int.MaxValue; // allow sorting if desired
        public Type? TypeOverride { get; set; }

        public BulkColumnAttribute(string? name = null)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Strongly typed column mapping for T -> DataTable -> SqlBulkCopy.
    /// </summary>
    public sealed class ColumnMap<T>
    {
        public sealed class Column
        {
            public string Name { get; }
            public Type DataType { get; }
            public Func<T, object?> Getter { get; }

            public Column(string name, Type type, Func<T, object?> getter)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                DataType = type ?? throw new ArgumentNullException(nameof(type));
                Getter = getter ?? throw new ArgumentNullException(nameof(getter));
            }
        }

        private readonly List<Column> _columns = new();
        public IReadOnlyList<Column> Columns => _columns;

        public ColumnMap<T> Add(string name, Type dataType, Func<T, object?> getter)
        {
            _columns.Add(new Column(name, dataType, getter));
            return this;
        }

        /// <summary>
        /// Convenience overload using expression to locate the property and infer its type.
        /// </summary>
        public ColumnMap<T> Add<TProp>(Expression<Func<T, TProp>> propExpr, string? columnName = null, Type? overrideType = null)
        {
            if (propExpr is null) throw new ArgumentNullException(nameof(propExpr));

            var member = (propExpr.Body as MemberExpression)?.Member as PropertyInfo
                         ?? throw new ArgumentException("Expression must target a property.", nameof(propExpr));

            var name = columnName ?? member.Name;
            var type = overrideType ?? Nullable.GetUnderlyingType(member.PropertyType) ?? member.PropertyType;

            // compile getter
            var getter = propExpr.Compile();
            Func<T, object?> g = x => (object?)getter(x) ?? DBNull.Value;

            return Add(name, type, g);
        }

        /// <summary>
        /// Auto-generate map from [BulkColumn] attributes (ordered by Order).
        /// </summary>
        public static ColumnMap<T> FromAttributes()
        {
            var map = new ColumnMap<T>();
            var props = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                var attr = p.GetCustomAttribute<BulkColumnAttribute>();
                if (attr == null) continue;

                var name = string.IsNullOrWhiteSpace(attr.Name) ? p.Name : attr.Name!;
                var type = attr.TypeOverride ?? Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

                var param = Expression.Parameter(typeof(T), "x");
                var body = Expression.Property(param, p);
                var lambda = Expression.Lambda(body, param); // returns object if boxed, we’ll convert below

                var compiled = lambda.Compile();
                object? Getter(T x) => compiled.DynamicInvoke(x);

                map.Add(name, type, Getter);
            }

            // Re-order if any Order overrides
            var ordered = typeof(T).GetProperties()
                .Select(p => (Prop: p, Attr: p.GetCustomAttribute<BulkColumnAttribute>()))
                .Where(x => x.Attr != null)
                .OrderBy(x => x.Attr!.Order)
                .ToList();

            if (ordered.Count > 0)
            {
                // rebuild in the declared order
                var remap = new ColumnMap<T>();
                foreach (var item in ordered)
                {
                    var attr = item.Attr!;
                    var name = string.IsNullOrWhiteSpace(attr.Name) ? item.Prop.Name : attr.Name!;
                    var type = attr.TypeOverride ?? Nullable.GetUnderlyingType(item.Prop.PropertyType) ?? item.Prop.PropertyType;

                    var param = Expression.Parameter(typeof(T), "x");
                    var body = Expression.Property(param, item.Prop);
                    var lambda = Expression.Lambda(body, param);
                    var compiled = lambda.Compile();
                    object? Getter(T x) => compiled.DynamicInvoke(x);

                    remap.Add(name, type, Getter);
                }
                return remap;
            }

            return map;
        }
    }
}
