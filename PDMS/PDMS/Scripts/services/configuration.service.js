//for configuration/referenceData

let ConfigurationService = {

    ConfigurationBaseUrl: 'Configuration',

    //REGION REFERENCE DATA
    
    /**
     * Gets all data fix table items
     * @param {Function} callback - Success callback 
     */
    getDataFixTableList: function (callback) {
        let url = `${this.ConfigurationBaseUrl}/GetAllDataFixTables`;

        ApiService.get(url, callback);

        console.log("getDataFixTableList called");
    },

    /**
     * Takes row id and Executes a callback function with the given value.
     * @param {*} rowId - Data Fix row id to lookup.
     * @param {Function} callback - Success callback
     */
    getDataFixDataItemForEdit: function (rowId, callback) {
        let url = `${this.ConfigurationBaseUrl}/GetRowItemById?id=${rowId}`;

        ApiService.get(url, callback);
    },

    /**
     * Deletes row by row id
     * @param {*} rowId - Data Fix row id.
     * @param {Function} callback - Success callback
     */
    deleteRowItemById: function (id, callback) {
        let url = `${this.ConfigurationBaseUrl}/DeleteRowItemById`;
        ApiService.post(url, id, callback);
    },

    /**
     * Saves row id or if 0 insert new record
     * @param {*} item - Data Fix item .
     * @param {Function} callback - Success callback
     * @Not implemented but setup for quick transform to front end technology
     */
    saveRowItem: function (item, callback) {
        let url = `${this.ConfigurationBaseUrl}/SaveRowItem`;

        ApiService.post(url, item, callback);
    },
    //END REGION REFERENCE DATA

    //REGION CONFIGURATION DATA




    //END REGION CONFIGURATION DATA
    getDataConfigurationTableData(tableName) {
        let table;

        let url = `${this.ConfigurationBaseUrl}/GetDataConfigurationTableData`;

        ApiService.post(url, tableName, callback);
    },

};