using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;


namespace Thesis.Entities
{
    public class ThesisObjectContext : AxiomEntities, IDisposable
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new AxiomEntities object using the connection string found in the 'AxiomEntities' section of the application configuration file.
        /// </summary>
        public ThesisObjectContext() : base() { }
    
        /// <summary>
        /// Initialize a new AxiomEntities object.
        /// </summary>
        public ThesisObjectContext(string connectionString) : base(connectionString) { }
    
        /// <summary>
        /// Initialize a new AxiomEntities object.
        /// </summary>
        public ThesisObjectContext(EntityConnection connection) : base(connection) { }
    
        #endregion
    }
}
