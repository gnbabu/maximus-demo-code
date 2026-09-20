//for configuration/referenceData

let ConfigurationDataService = {

    ConfigurationBaseUrl: 'ConfigurationData',

    //REGION REFERENCE DATA

    loadConfigurationDataDropDownItems: function (callback) {
        let url = `${this.ConfigurationBaseUrl}/GetConfigurationDataTablesList`;
        ApiService.get(url, callback);

        console.log('loadConfigurationDataDropDownItems');
    },

    /**
     * Gets all configuration table data items
     * @param {Function} callback - Success callback 
     */
    getConfigurationTableRows: function (tableName,  callback) {
        let url = `${this.ConfigurationBaseUrl}/GetConfigurationDataTablesRows`;

        ApiService.post(url, tableName, callback);

        console.log("getConfigurationTablesRows called");
    },

    /**
     * Takes row id and Executes a callback function with the given value.
     * @param {*} rowId - Configuration data row id to lookup.
     * @param {Function} callback - Success callback
     */
    getConfigurationDataItemForEdit: function (rowId, callback) {
        let url = `${this.ConfigurationBaseUrl}/GetConfigurationDataRowItemById?id=${rowId}`;

        ApiService.get(url, callback);
    },

    /**
     * Deletes row by row id
     * @param {*} rowId - Configuration data row id.
     * @param {Function} callback - Success callback
     */
    deleteConfigurationDataRowItemById: function (id, tableCtx, callback) {
        let url = `${this.ConfigurationBaseUrl}/DeleteConfigurationDataRowItemById`;
        ApiService.post(url, id, callback);
    },

    /**
     * Saves row id or if 0 insert new record
     * @param {*} item - Table entityt item .
     * @param {Function} callback - Success callback
     * @Not implemented but setup for quick transform to front end technology
     */
    saveConfigurationDataItem: function (dtoObject, callback) {
        let url = `${this.ConfigurationBaseUrl}/SaveConfigurationDataItem`;

        ApiService.post(url, dtoObject, callback);
    },
    //END REGION REFERENCE DATA

    //REGION CONFIGURATION DATA



    getDataFixTablePropertiesByTableName(tableName, callback) {
        let url = `${this.ConfigurationBaseUrl}/GetDataFixTablePropertiesByTableName`;

        ApiService.post(url, tableName, callback);
    },


    //END REGION CONFIGURATION DATA
    getDataConfigurationTableData(tableName) {
        
        let url = `${this.ConfigurationBaseUrl}/GetDataConfigurationTableData`;

        ApiService.post(url, tableName, callback);
    },

};