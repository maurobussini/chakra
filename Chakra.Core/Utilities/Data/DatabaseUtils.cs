using System;
using System.Data;
using System.Data.Common;

namespace ZenProgramming.Chakra.Core.Utilities.Data
{
    /// <summary>
    /// Utilities for interact with database
    /// </summary>
    public static class DatabaseUtils
    {
        /// <summary>
        /// Execute generation of database provider factory using his name
        /// </summary>
        /// <param name="providerInvariantName">Provider invariant name</param>
        /// <returns>Return provider factory instance</returns>
        public static DbProviderFactory GetProviderFactory(string providerInvariantName)
        {
            //Eseguo la validazione degli argomenti
            if (string.IsNullOrWhiteSpace(providerInvariantName)) throw new ArgumentNullException(nameof(providerInvariantName));

            //Creo il tipo del provider da utilizzare
            Type providerType = Type.GetType(providerInvariantName);
            if (providerType == null)
                throw new InvalidOperationException($"No provider for '{providerInvariantName}' type");

            //Utilizzo la reflection per il recupero
            object instance = Activator.CreateInstance(providerType);
            if (instance == null)
                throw new InvalidOperationException($"Instance for '{providerInvariantName}' cannot be created");

            //Tentativo di casting
            if (!(instance is DbProviderFactory provider))
                throw new InvalidOperationException($"Instance of '{providerInvariantName}' is not a valid provider factory");

            //Ritorno l'istanza
            return provider;
        }

        /// <summary>
        /// Execute creation and opening of connection, using provider
        /// and connectionstring specified as parameters
        /// </summary>
        /// <param name="provider">Database provider</param>
        /// <param name="connectionString">Connectionstring to use</param>
        /// <returns>Return opened connection</returns>
        public static DbConnection CreateConnection(DbProviderFactory provider, string connectionString)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            //Eseguo la creazione di un nuovo oggetto di connessione
            DbConnection connection = provider.CreateConnection();

            //Se la connessione è nulla, emetto eccezione
            if (connection == null)
                throw new NullReferenceException("Unable to generate a valid database " +
                                                 $"connection using provider '{provider.GetType().FullName}'");

            //Imposto la connectiostring passata e tento l'apertura
            connection.ConnectionString = connectionString;
            connection.Open();

            //Se la connessione è chiusa (o rotta), emetto eccezione
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                throw new InvalidOperationException(
                    $"Connection with database is not opened: current state is '{connection.State}'");

            //Ritorno la connessione aperta
            return connection;
        }

        /// <summary>
        /// Create a parameter for specified provider
        /// </summary>
        /// <param name="provider">Reference provider</param>
        /// <param name="name">Name of parameter</param>
        /// <param name="type">Type of parameter</param>
        /// <param name="value">Value of parameter</param>
        /// <returns>Returns a parameter</returns>
        public static DbParameter CreateParameter(DbProviderFactory provider, string name, DbType type, object value)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            //Imposto l'oggetto command per l'esecuzione dell'adapter
            DbParameter param = provider.CreateParameter();

            //Se il parametro è nullo, emetto eccezione
            if (param == null)
                throw new NullReferenceException("Unable to generate a valid parameter " +
                                                 $"using provider '{provider.GetType().FullName}'");

            //Inserisco nell'oggetto le informazioni necessarie
            param.DbType = type;
            param.ParameterName = "@" + name.Replace("@", "");
            param.Value = value;
            return param;
        }

        /// <summary>
        /// Create a data adapter using specified properties
        /// </summary>
        /// <param name="provider">Database provider</param>
        /// <param name="connection">Opened connection</param>
        /// <param name="query">Query to retrieve data</param>
        /// <param name="parameters">Parameters of query</param>
        /// <returns>Return a data adapter with data</returns>
        public static DbDataAdapter CreateDataAdapter(DbProviderFactory provider, DbConnection connection,
            string query, DbParameter[] parameters = null)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            //Se la connessione non è aperta, emetto eccezione
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Unable to generate a valid data adapter: " +
                                                    $"connection state is '{connection.State}'.");

            //Creo un nuovo oggetto DataAdapter sulla connessione e query passate
            DbDataAdapter dataAdapter = provider.CreateDataAdapter();

            //Se l'elemento è nullo emetto eccezione
            if (dataAdapter == null)
                throw new NullReferenceException("Unable to create a valid 'DbDataAdapter'");

            //Imposto l'oggetto command per l'esecuzione dell'adapter
            using (DbCommand selectCommand = provider.CreateCommand())
            {
                //Se il comando è nullo, emetto eccezione
                if (selectCommand == null)
                    throw new NullReferenceException("Unable to create instance of 'DbCommand'");

                //Nessun timepout della connessione
                selectCommand.CommandTimeout = 0;

                //Imposto query e connessione                
                selectCommand.CommandText = query;
                selectCommand.Connection = connection;

                //Scorro l'elenco dei parametri e li associo al command (se specificati)
                if (parameters != null)
                    foreach (DbParameter t in parameters)
                        selectCommand.Parameters.Add(t);

                //Imposto il comando di selezione
                dataAdapter.SelectCommand = selectCommand;
            }

            //Ritorno l'adattatore
            return dataAdapter;
        }

        /// <summary>
        /// Create a dataset using data contained in the adapter
        /// </summary>
        /// <param name="dataAdapter">Adapter to use</param>
        /// <param name="fillSchema">Specify if data schema must be recovered</param>
        /// <returns>Return a dataset</returns>
        public static DataSet CreateDataSet(DbDataAdapter dataAdapter, bool fillSchema = true)
        {
            //Verifico che l'adatattatore non sia nullo
            if (dataAdapter == null) throw new ArgumentNullException(nameof(dataAdapter));

            //Creo un nuovo oggetto DataTable vuoto
            DataSet dataSet = new DataSet();

            //Se ho specificato di importare lo schema
            if (fillSchema)
                dataAdapter.FillSchema(dataSet, SchemaType.Mapped);

            //Eseguo il riempimento del datase            
            dataAdapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Create a datatable using data contained in the adapter
        /// </summary>
        /// <param name="dataAdapter">Adapter to use</param>
        /// <param name="fillSchema">Specify if data schema must be recovered</param>
        /// <returns>Return a datatable</returns>
        public static DataTable CreateDataTable(DbDataAdapter dataAdapter, bool fillSchema = true)
        {
            //Verifico che l'adatattatore non sia nullo
            if (dataAdapter == null) throw new ArgumentNullException(nameof(dataAdapter));

            //Creo un nuovo oggetto DataTable vuoto
            DataTable dataTable = new DataTable();

            //Se ho specificato di importare lo schema
            if (fillSchema)
                dataAdapter.FillSchema(dataTable, SchemaType.Source);

            //Eseguo il riempimento del datase            
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Execute a query over database connection passed
        /// </summary>
        /// <param name="provider">Database provider</param>
        /// <param name="connection">Opened connection</param>
        /// <param name="query">Instruction to launch</param>
        /// <param name="parameters">Optional parameters</param>
        /// <param name="transaction">Optional transaction</param>
        /// <returns>Return number of rows effected</returns>
        public static int ExecuteNonQuery(DbProviderFactory provider, DbConnection connection, string query,
            DbParameter[] parameters = null, DbTransaction transaction = null)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            //Se la connessione non è aperta, emetto eccezione
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Unable to execute the statement on database: " +
                                                    $"connection state is '{connection.State}'.");

            //Predispongo la variabile di ritorno
            int resultValue;

            //Creo un nuovo oggetto DbCommand sulla connessione e query passate
            using (DbCommand command = provider.CreateCommand())
            {
                //Se il comando è nullo, emetto eccezione
                if (command == null)
                    throw new NullReferenceException("Unable to create instance of 'DbCommand'");

                //Imposto la connessione e l'istruzione
                command.Connection = connection;
                command.CommandText = query;

                //Se esiste una transazione attiva, la passo al command
                if (transaction != null)
                    command.Transaction = transaction;

                //Scorro l'elenco dei parametri e li associo al command (se specificati)
                if (parameters != null)
                    foreach (DbParameter t in parameters)
                        command.Parameters.Add(t);

                //Eseguo l'istruzione per l'avvio dell'aggiornamento
                resultValue = command.ExecuteNonQuery();
            }

            //Ritorno il valore 
            return resultValue;
        }

        /// <summary>
        /// Create a reader used to access data matching criteria
        /// </summary>
        /// <param name="provider">Database provider</param>
        /// <param name="connection">Opened connection</param>
        /// <param name="query">Instruction to launch</param>
        /// <param name="parameters">Optional parameters</param>
        /// <param name="transaction">Optional transaction</param>
        /// <returns>Returns reader</returns>
        public static DbDataReader CreateDataReader(DbProviderFactory provider, DbConnection connection, string query,
            DbParameter[] parameters = null, DbTransaction transaction = null)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            //Se la connessione non è aperta, emetto eccezione
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Unable to execute the statement on database: " +
                                                    $"connection state is '{connection.State}'.");

            //Creo un nuovo oggetto DbCommand sulla connessione e query passate
            using (DbCommand command = provider.CreateCommand())
            {
                //Se il comando è nullo, emetto eccezione
                if (command == null)
                    throw new NullReferenceException("Unable to create instance of 'DbCommand'");

                //Imposto la connessione e l'istruzione
                command.Connection = connection;
                command.CommandText = query;

                //Se esiste una transazione attiva, la passo al command
                if (transaction != null)
                    command.Transaction = transaction;

                //Scorro l'elenco dei parametri e li associo al command (se specificati)
                if (parameters != null)
                    foreach (DbParameter t in parameters)
                        command.Parameters.Add(t);

                //Ritorno l'istanza del reader
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Execute a statement on database, recovering scalar data
        /// </summary>
        /// <param name="provider">Database provider</param>
        /// <param name="connection">Opened connection</param>
        /// <param name="query">Statement to execute</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>Return extracted value</returns>
        public static object ExecuteScalar(DbProviderFactory provider, DbConnection connection,
            string query, DbParameter[] parameters = null)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            //Se la connessione non è aperta, emetto eccezione
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Unable to recover a scalar data: " +
                                                    $"connection state is '{connection.State}'.");

            //Predispongo la variabile di ritorno
            object resultValue;

            //Creo un nuovo oggetto DbCommand sulla connessione e query passate
            using (DbCommand command = provider.CreateCommand())
            {
                //Se il comando è nullo, emetto eccezione
                if (command == null)
                    throw new NullReferenceException("Unable to create instance of 'DbCommand'");

                //Imposto la connessione e la query
                command.Connection = connection;
                command.CommandText = query;

                //Scorro l'elenco dei parametri e li associo al command (se specificati)
                if (parameters != null)
                    foreach (DbParameter t in parameters)
                        command.Parameters.Add(t);

                //Eseguo l'estrazione scalare del dato
                resultValue = command.ExecuteScalar();
            }

            //Eseguo il ritorno del valore
            return resultValue;
        }

        /// <summary>
        /// Execute update of data on storage, using original extraction statemente,
        /// datatable with modified data, provider and connection to database
        /// </summary>
        /// <param name="provider">Reference provider</param>
        /// <param name="connection">Opened connection</param>
        /// <param name="modifiedTable">Modified table of data</param>
        /// <param name="originalQuery">Original statement</param>
        /// <param name="parameters">Optional parameters</param>
        /// <param name="transaction">Active transaction </param>
        /// <returns>Returns number of rows affected</returns>
        public static int UpdateData(DbProviderFactory provider, DbConnection connection, DataTable modifiedTable, string originalQuery,
            DbParameter[] parameters = null, DbTransaction transaction = null)
        {
            //Verifico che i parametri specificati siano validi
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(originalQuery)) throw new ArgumentNullException(nameof(originalQuery));
            if (modifiedTable == null) throw new ArgumentNullException(nameof(modifiedTable));

            //Se la connessione non è aperta, emetto eccezione
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Unable to data on storage: " +
                                                    $"connection state is '{connection.State}'.");

            //Predispongo il numero di righe "affected"
            int rowsAffected;

            //Creo un nuovo oggetto DataAdapter sulla connessione e query passate
            //e il relativo costruttore di comandi T-SQL per eseguire l'aggiornamento
            using (DbDataAdapter dataAdapter = provider.CreateDataAdapter())
            {
                //Se l'elemento è nullo emetto eccezione
                if (dataAdapter == null)
                    throw new NullReferenceException("Unable to create a valid 'DbDataAdapter'");

                //Imposto l'oggetto command per l'esecuzione dell'adapter
                using (DbCommand selectCommand = provider.CreateCommand())
                {
                    //Se il comando è nullo, emetto eccezione
                    if (selectCommand == null)
                        throw new NullReferenceException("Unable to create instance of 'DbCommand'");

                    //Imposto la query e la connessione, quindi associo al dataadapter
                    selectCommand.CommandText = originalQuery;
                    selectCommand.Connection = connection;
                    dataAdapter.SelectCommand = selectCommand;

                    //Creo un commandbuilder e gli associo l'adapter
                    using (DbCommandBuilder commandBuilder = provider.CreateCommandBuilder())
                    {
                        //Se l'elemento è nullo emetto eccezione
                        if (commandBuilder == null)
                            throw new NullReferenceException("Unable to create a valid 'DbCommandBuilder'");

                        //Imposto l'adattatore nel comando
                        commandBuilder.DataAdapter = dataAdapter;

                        //Scorro l'elenco dei parametri e li associo al command (se specificati)
                        if (parameters != null)
                            foreach (DbParameter t in parameters)
                                selectCommand.Parameters.Add(t);

                        //Se è stata passata una transazione attiva al metodo,
                        //perchè venga realizzata è necessario che sia impostata sul command
                        //per tutte le modalità attive (inserimento, modifica, eliminazione, selezione)
                        if (transaction != null)
                        {
                            //Assegno la transazione solo ai comandi non nulli
                            dataAdapter.SelectCommand.Transaction = transaction;
                            if (dataAdapter.InsertCommand != null) dataAdapter.InsertCommand.Transaction = transaction;
                            if (dataAdapter.UpdateCommand != null) dataAdapter.UpdateCommand.Transaction = transaction;
                            if (dataAdapter.DeleteCommand != null) dataAdapter.DeleteCommand.Transaction = transaction;
                        }

                        //Eseguo un update mediante differenze su datatable modificata
                        rowsAffected = dataAdapter.Update(modifiedTable);
                    }
                }
            }

            //Ritorno il numero di righe modificate
            return rowsAffected;
        }


        /// <summary>
        /// Slice specified DataTable in array of DataTable with same structure
        /// but less rows; number of rows for item is specified as second parameter
        /// </summary>
        /// <param name="source">Source DataTable</param>
        /// <param name="maxRows">Max row for item</param>
        /// <returns>Returns array of DataTable</returns>
        public static DataTable[] SliceDataTable(DataTable source, int maxRows)
        {
            //Se non è stata passata una sorgente dati, emetto eccezione
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (maxRows <= 0) throw new ArgumentOutOfRangeException(string.Format("Parameter 'maxRows' must have a value major or equals to 1."));

            //Se il numero di righe contenute nella sorgente è 0, la ritorno
            if (source.Rows.Count == 0) return new[] { source };

            //Calcolo il numero delle DataTable necessarie per contenere
            //tutte le informazioni presenti nella DataTable sorgente; per il
            //calcolo utilizzo la funzione che esegue una divisione e esporta
            //il resto (eventuale) in una variabile di output. Se esiste un resto
            //è necessario che il numero delle tabelle sia incrementato di uno
            int remainingCount;
            int tablesNeeded = Math.DivRem(source.Rows.Count, maxRows, out remainingCount);
            tablesNeeded = (remainingCount > 0) ? tablesNeeded + 1 : tablesNeeded;

            //Dimensiono un array di DataTable per l'uscita
            DataTable[] arrDataTables = new DataTable[tablesNeeded];

            //Se è necessaria una sola tabella per l'inserimento delle
            //informazioni in uscita, semplicemente passo quella sorgente
            if (tablesNeeded == 1)
            {
                //Inserisco in uscita la tabella sorgente
                arrDataTables[0] = source;
            }
            else
            {
                //Se invece è necessario eseguire un "taglio" delle informazioni su
                //più tabelle, procedo con un ciclo su tutte le tabelle destinatarie delle informazioni
                for (int i = 0; i < tablesNeeded; i++)
                {
                    //Creo una nuova DataTable eseguendo una clonazione della struttura
                    DataTable currentTarget = source.Clone();

                    //Per ciascuna tabella scorro le righe sorgenti (solo la porzione
                    //che mi interessa) e le importo copiandole nella destinazione
                    for (int n = 0; n < maxRows && (((i * maxRows) + n) < source.Rows.Count); n++)
                        currentTarget.ImportRow(source.Rows[(i * maxRows) + n]);

                    //Inserisco la DataTable nell'array di uscita
                    arrDataTables[i] = currentTarget;
                }
            }

            //Ritorno i uscita l'array composto
            return arrDataTables;
        }

        /// <summary>
        /// Retrieve value of specified field on datarow converting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="sourceRow">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <returns>Returns retrieved value</returns>
        public static TTarget RetrieveValue<TTarget>(DataRow sourceRow, string fieldName)
        {
            //Eseguo la validazione degli argomenti
            if (sourceRow == null) throw new ArgumentNullException(nameof(sourceRow));
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Utilizzo il metodo overloaded utilizzando il default del tipo
            return RetrieveValue(sourceRow, fieldName, default(TTarget));
        }

        /// <summary>
        /// Retrieve value of specified field on datarow converting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="sourceRow">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <param name="defaultValue">Default value if source value is DBNull</param>
        /// <returns>Returns retrieved value</returns>
        public static TTarget RetrieveValue<TTarget>(DataRow sourceRow, string fieldName, TTarget defaultValue)
        {
            //Eseguo la validazione degli argomenti
            if (sourceRow == null) throw new ArgumentNullException(nameof(sourceRow));
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Eseguo il recupero del valore indicato
            object sourceValue = sourceRow[fieldName];

            //Se il tipo di uscita è nullabile, recupero il suo primitivo, altrimenti quello originale
            Type primitiveType = Nullable.GetUnderlyingType(typeof(TTarget)) ?? typeof(TTarget);

            //Se il valore recuperato è nullo, ritorno
            //il valore impostato come default, altrimenti
            //eseguo un cambiamento del tipo verso "T"
            return (sourceValue == DBNull.Value) ?
                defaultValue : (TTarget)Convert.ChangeType(sourceValue, primitiveType);
        }

        /// <summary>
        /// Retrieve value of specified field from datareader converting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="reader">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <returns>Returns retrieved value</returns>
        public static TTarget RetrieveValue<TTarget>(IDataReader reader, string fieldName)
        {
            //Eseguo la validazione degli argomenti
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Utilizzo il metodo overloaded utilizzando il default del tipo
            return RetrieveValue(reader, fieldName, default(TTarget));
        }

        /// <summary>
        /// Retrieve value of specified field from datareader converting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="reader">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <param name="defaultValue">Default value if source value is DBNull</param>
        /// <returns>Returns retrieved value</returns>
        public static TTarget RetrieveValue<TTarget>(IDataReader reader, string fieldName, TTarget defaultValue)
        {
            //Eseguo la validazione degli argomenti
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

            //Recupero la posizione della colonna di nome specificato
            int ordinal = reader.GetOrdinal(fieldName);

            //Se l'ordinale è minore di 0, emetto eccezione (non trovata)
            if (ordinal < 0) throw new InvalidOperationException($"Column '{fieldName}' not found on datareader.");

            //Eseguo il recupero del valore indicato
            object sourceValue = reader.GetValue(ordinal);

            //Se il tipo di uscita è nullabile, recupero il suo primitivo, altrimenti quello originale
            Type primitiveType = Nullable.GetUnderlyingType(typeof(TTarget)) ?? typeof(TTarget);

            //Se il valore è DbNull, ritorno il default
            if (sourceValue == DBNull.Value)
                return defaultValue;

            //Se il valore è un'enumerazione, tento la conversione
            if (typeof(TTarget).IsEnum)
                return (TTarget)Enum.ToObject(typeof(TTarget), sourceValue);

            //Eseguo la conversione del valore sul tipo target
            return (TTarget)Convert.ChangeType(sourceValue, primitiveType);
        }

        /// <summary>
        /// Tryes to retrieve value of specified field from datarow, conveting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="sourceRow">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <param name="result">Output result</param>
        /// <returns>Returns true if conversion was successfull</returns>
        public static bool TryRetrieveValue<TTarget>(DataRow sourceRow, String fieldName, out TTarget result)
        {
            //Utilizzo il metodo overloaded utilizzando il default del tipo
            return TryRetrieveValue(sourceRow, fieldName, out result, default(TTarget));
        }

        /// <summary>
        /// Tryes to retrieve value of specified field from datarow, conveting value to output type
        /// </summary>
        /// <typeparam name="TTarget">Type of result value</typeparam>
        /// <param name="sourceRow">Source datarow</param>
        /// <param name="fieldName">Source field name</param>
        /// <param name="result">Output result</param>
        /// <param name="defaultValue">Default value if source value is DBNull</param>
        /// <returns>Returns true if conversion was successfull</returns>
        public static bool TryRetrieveValue<TTarget>(DataRow sourceRow, String fieldName, out TTarget result, TTarget defaultValue)
        {
            try
            {
                //Eseguo il recupero del valore specificato
                result = RetrieveValue(sourceRow, fieldName, defaultValue);

                //Ritorno conferma di recupero dati
                return true;
            }
            catch
            {
                //Assegno il valore di default all'uscita
                result = default(TTarget);

                //Ritorno fallimento
                return false;
            }
        }
    }
}
