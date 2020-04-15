
namespace Data {
    public sealed class DataStore {
        private static readonly DataStore instance = new DataStore();
        private AdjacencyRules rules = new AdjacencyRules();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DataStore()
        {
        }

        private DataStore()
        {
        }

        public static DataStore Instance 
        {
            get
            {
                return instance;
            }
        }

        public AdjacencyRules Rules {
            get {
                return rules;
            }
        }
    }
}