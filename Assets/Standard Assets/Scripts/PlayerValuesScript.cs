using UnityEngine;
using System.Collections;

public class PlayerValuesScript : MonoBehaviour {
void Start(){
//Player Specific Info
PlayerPrefs.GetString("Player Name", "Player");
PlayerPrefs.GetInt("Cash", 0);
PlayerPrefs.GetInt("Highest Score - Trick", 0);
PlayerPrefs.GetInt("Highest Score - Round", 0);
PlayerPrefs.GetInt("Total Score", 0);
PlayerPrefs.GetString("Driving Level", "Amateur");
		
//Player Truck Info
PlayerPrefs.GetString("Monster Truck Name", "Truck");
PlayerPrefs.GetInt("Speed", 27);
PlayerPrefs.GetInt("Handling", 27);
PlayerPrefs.GetInt("Durability", 27);
PlayerPrefs.GetInt("Weight", 42);
//Need to enumerate for color of truck
PlayerPrefs.GetInt("TruckColor", 0);
		
//Player Upgrades Info
PlayerPrefs.GetInt("Air Filter Street Purchased", 0);
PlayerPrefs.GetInt("Air Filter Street Active", 0);
PlayerPrefs.GetInt("Air Filter Sport Purchased", 0);
PlayerPrefs.GetInt("Air Filter Sport Active", 0);
PlayerPrefs.GetInt("Air Filter Race Purchased", 0);
PlayerPrefs.GetInt("Air Filter Race Active", 0);
		
PlayerPrefs.GetInt("Camshaft Street Purchased", 0);
PlayerPrefs.GetInt("Camshaft Street Active", 0);
PlayerPrefs.GetInt("Camshaft Sport Purchased", 0);
PlayerPrefs.GetInt("Camshaft Sport Active", 0);
PlayerPrefs.GetInt("Camshaft Race Purchased", 0);
PlayerPrefs.GetInt("Camshaft Race Active", 0);
		
PlayerPrefs.GetInt("Displacement Street Purchased", 0);
PlayerPrefs.GetInt("Displacement Street Active", 0);
PlayerPrefs.GetInt("Displacement Sport Purchased", 0);
PlayerPrefs.GetInt("Displacement Sport Active", 0);
PlayerPrefs.GetInt("Displacement Race Purchased", 0);
PlayerPrefs.GetInt("Displacement Race Active", 0);

PlayerPrefs.GetInt("Exhaust Street Purchased", 0);
PlayerPrefs.GetInt("Exhaust Street Active", 0);
PlayerPrefs.GetInt("Exhaust Sport Purchased", 0);
PlayerPrefs.GetInt("Exhaust Sport Active", 0);
PlayerPrefs.GetInt("Exhaust Race Purchased", 0);
PlayerPrefs.GetInt("Exhaust Race Active", 0);
		
PlayerPrefs.GetInt("Fuel Street Purchased", 0);
PlayerPrefs.GetInt("Fuel Street Active", 0);
PlayerPrefs.GetInt("Fuel Sport Purchased", 0);
PlayerPrefs.GetInt("Fuel Sport Active", 0);
PlayerPrefs.GetInt("Fuel Race Purchased", 0);
PlayerPrefs.GetInt("Fuel Race Active", 0);
		
PlayerPrefs.GetInt("Ignition Street Purchased", 0);
PlayerPrefs.GetInt("Ignition Street Active", 0);
PlayerPrefs.GetInt("Ignition Sport Purchased", 0);
PlayerPrefs.GetInt("Ignition Sport Active", 0);
PlayerPrefs.GetInt("Ignition Race Purchased", 0);
PlayerPrefs.GetInt("Ignition Race Active", 0);
		
PlayerPrefs.GetInt("IntakeManifold Street Purchased", 0);
PlayerPrefs.GetInt("IntakeManifold Street Active", 0);
PlayerPrefs.GetInt("IntakeManifold Sport Purchased", 0);
PlayerPrefs.GetInt("IntakeManifold Sport Active", 0);
PlayerPrefs.GetInt("IntakeManifold Race Purchased", 0);
PlayerPrefs.GetInt("IntakeManifold Race Active", 0);
		
PlayerPrefs.GetInt("Piston Street Purchased", 0);
PlayerPrefs.GetInt("Piston Street Active", 0);
PlayerPrefs.GetInt("Piston Sport Purchased", 0);
PlayerPrefs.GetInt("Piston Sport Active", 0);
PlayerPrefs.GetInt("Piston Race Purchased", 0);
PlayerPrefs.GetInt("Piston Race Active", 0);
		
PlayerPrefs.GetInt("SuperCharger Street Purchased", 0);
PlayerPrefs.GetInt("SuperCharger Street Active", 0);
PlayerPrefs.GetInt("SuperCharger Sport Purchased", 0);
PlayerPrefs.GetInt("SuperCharger Sport Active", 0);
PlayerPrefs.GetInt("SuperCharger Race Purchased", 0);
PlayerPrefs.GetInt("SuperCharger Race Active", 0);
		
PlayerPrefs.GetInt("RollCage Street Purchased", 0);
PlayerPrefs.GetInt("RollCage Street Active", 0);
PlayerPrefs.GetInt("RollCage Sport Purchased", 0);
PlayerPrefs.GetInt("RollCage Sport Active", 0);
PlayerPrefs.GetInt("RollCage Race Purchased", 0);
PlayerPrefs.GetInt("RollCage Race Active", 0);
		
PlayerPrefs.GetInt("Spoiler Street Purchased", 0);
PlayerPrefs.GetInt("Spoiler Street Active", 0);
PlayerPrefs.GetInt("Spoiler Sport Purchased", 0);
PlayerPrefs.GetInt("Spoiler Sport Active", 0);
PlayerPrefs.GetInt("Spoiler Race Purchased", 0);
PlayerPrefs.GetInt("Spoiler Race Active", 0);

PlayerPrefs.GetInt("Suspension Street Purchased", 0);
PlayerPrefs.GetInt("Suspension Street Active", 0);
PlayerPrefs.GetInt("Suspension Sport Purchased", 0);
PlayerPrefs.GetInt("Suspension Sport Active", 0);
PlayerPrefs.GetInt("Suspension Race Purchased", 0);
PlayerPrefs.GetInt("Suspension Race Active", 0);
		
PlayerPrefs.GetInt("Tires Street Purchased", 0);
PlayerPrefs.GetInt("Tires Street Active", 0);
PlayerPrefs.GetInt("Tires Sport Purchased", 0);
PlayerPrefs.GetInt("Tires Sport Active", 0);
PlayerPrefs.GetInt("Tires Race Purchased", 0);
PlayerPrefs.GetInt("Tires Race Active", 0);
		
PlayerPrefs.GetInt("WeightReduction Street Purchased", 0);
PlayerPrefs.GetInt("WeightReduction Street Active", 0);
PlayerPrefs.GetInt("WeightReduction Sport Purchased", 0);
PlayerPrefs.GetInt("WeightReduction Sport Active", 0);
PlayerPrefs.GetInt("WeightReduction Race Purchased", 0);
PlayerPrefs.GetInt("WeightReduction Race Active", 0);
		
	}
}
