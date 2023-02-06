using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Logic mockups. Does nothing with the generated stars

public class Galaxy : MonoBehaviour
{
    public List<StarNode> stars = new List<StarNode>();
    public int size;
    public GalaxyGenerator(int amount){
        for(int i = 0; i < amount; i++){
            stars.Add(CreateStar())
        }

        //creats hyperlane connections
        foreach(StarNode hyperRoot in stars){
            foreach(StarNode hyperTarget in stars){
                // if the target star is the current star or already has a connection to the current star
                if(hyperTarget == hyperRoot || hyperTarget.Contains(hyperRoot)) break; 
                //If the stars are close enough together 
                if(Vector3.Distance(hyperRoot, hyperTarget) < 5){
                    hyperRoot.hyperlaneNeighbors.Add(hyperTarget);
                    hyperTarget.hyperlaneNeighbors.Add(hyperRoot);
                }
            }
        }
        //checks so all stars have friends
        foreach(StarNode lonleyStar in stars){
            if(star.hyperlaneNeighbors.Count == 0){
                StarNode tempStarPointer = star[0]; //remembers the closest star so far and the distance
                int relationshipDistance = Vector3.Distance(lonleyStar.location, star[0].location);
                foreach(StarNode potentialFriend in stars){
                    int tempDistance = Vector3.Distance(lonleyStar.location, potentialFriendn.location);
                    if(relationshipDistance > tempDistance){ //if we find a closer star we make thet the new potential friend
                        tempStarPointer = potentialFriend;
                        relationshipDistance = tempDistance;
                    }
                }
            }
        }
    }
}

public class StarNode : MonoBehaviour
{
    static private Random randomGen = new Random();
    public string name;
    public var starmap; //placeholder for ANANAS
    public Vector3 location = Vector3;
    public List<StarNode> hyperlaneNeighbors = new List<StarNode>();

    static public StarNode CreateStar(){
        StarNode newStar;
        
        newStar.location.x = StarNode.randomGen.NextDouble()*100;
        newStar.location.y = StarNode.randomGen.NextDouble()*100;
        newStar.location.z = 0;

        return newStar;
    }
}