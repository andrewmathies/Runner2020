using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Obstacle {
    public class AdjacencyRules {
        private Dictionary<string, int> rules;

        public AdjacencyRules() {
            rules = new Dictionary<string, int>();

            rules.Add("BlueGreen", 2);
            rules.Add("BlackGreen", 2);
            rules.Add("BlackBlue", 2);

            rules.Add("BlueBlue", 3);
            rules.Add("GreenGreen", 3);
            rules.Add("BlackBlack", 3);
        } 

        // TODO: refactor
        // I'm storing rules on how close obstacles of different types can be to each
        // other as a map where the keys are strings concatenated together.
        
        // This function takes two types of obstacles and returns the key
        private int getRule(ObstacleType first, ObstacleType second) {
            string firstString = first.ToString();
            string secondString = second.ToString();
            string ruleString = "";

            if (string.Compare(firstString, secondString) < 0) {
                ruleString = firstString + secondString;
            } else {
                ruleString = secondString + firstString;
            }

            if (rules.ContainsKey(ruleString)) {
                return rules[ruleString];
            }

            throw new System.ArgumentException("need to add new rule to dictionary: " + first + " " + second);
        }

        // return a list of unique obstacle types that are valid to be placed in the current location
        // given the rules and distances to any obstacles in the scene
        public List<ObstacleType> Apply(Dictionary<ObstacleType, int> distances) {
            HashSet<ObstacleType> options = new HashSet<ObstacleType>();
            var types = Enum.GetValues(typeof(ObstacleType));
            
            foreach (ObstacleType firstType in types) {
                foreach (ObstacleType secondType in types) {
                    int minDistance = getRule(firstType, secondType);
                    
                    if (distances.ContainsKey(firstType)) {
                        int curDistance = distances[firstType];

                        if (curDistance <= minDistance) {
                            continue;
                        }
                    }

                    options.Add(firstType);
                }
            }

            return new List<ObstacleType>(options);
        }
    }
}

/*
current distance: 2 to black, 3 to blue
so answer is [green]

1       2
blue    blue
blue    black
blue    green
black   blue
black   black
black   green
green   blue
green   black
green   green

*/