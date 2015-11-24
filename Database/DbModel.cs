namespace Database
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DbModel : DbContext
    {
        // Your context has been configured to use a 'DbModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Database.DbModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DbModel' 
        // connection string in the application configuration file.
        public DbModel()
            : base("name=DbModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<TicTacToeResult> TicTacToeResult { get; set; }
    }

    public class TicTacToeResult
    {
        public int TicTacToeResultId { get; set; }
        public string Name { get; set; }
        public int HiddenLayerCount { get; set; }
        public int NeuronPerLayercount { get; set; }
        public string ActivationFunction { get; set; }
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public int BatchSize { get; set; }
        public int Epochs { get; set; }
        public double Error { get; set; }
        public int TrainingDataCount { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
    }
}