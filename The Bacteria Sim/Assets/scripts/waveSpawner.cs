﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
		
*/
public class waveSpawner : MonoBehaviour {

	public GameObject center;
	public GameObject GameManager;
	float widthOfWorld;
    float heightOfWorld;
    public int waveCount;
    public int waveNumber;
    int ennemyLevel;
    int typesOfEnnemies;
    public List<GameObject> ennemies;
	// Use this for initialization
	void Start () {
		waveCount = 0;
    	typesOfEnnemies = (int) Mathf.Floor(waveNumber/3); 
    	ennemyLevel = (int) Mathf.Floor(waveNumber/5);
		widthOfWorld = GameManager.GetComponent<gameManager>().widthOfWorld;
		heightOfWorld = GameManager.GetComponent<gameManager>().heightOfWorld;
	}
	
	// Update is called once per frame
	void Update () {
		if (waveCount <=0) {
			upgradeWave();
			createWave(typesOfEnnemies);
			waveNumber++;
		}
	}

	Vector2 getRandSpawnPoint(){
		int mainDir = Random.Range(0,4);
		float x = 0f;
		float y = 0f;
		float xScale = transform.localScale.x;
		float yScale = transform.localScale.y;
		//south
		if (mainDir == 0){
			x = Random.Range(transform.position.x - (xScale/2), transform.position.x + (widthOfWorld/2));
			y = Random.Range(transform.position.y - (yScale/2), transform.position.x - (heightOfWorld/2));
		}
		//east
		else if (mainDir == 1){
			x = Random.Range(transform.position.x + (widthOfWorld/2), transform.position.x + (xScale/2));
			y = Random.Range(transform.position.y - (yScale/2), transform.position.y + (heightOfWorld/2));
		}
		//north
		else if (mainDir == 2){
			x = Random.Range(transform.position.x - (widthOfWorld/2), transform.position.x + (xScale/2));
			y = Random.Range(transform.position.x + (heightOfWorld/2), transform.position.y + (yScale/2));
		}
		//west
		else if (mainDir == 3){
			x = Random.Range(transform.position.x - (xScale/2), transform.position.y - (widthOfWorld/2));
			y = Random.Range(transform.position.y - (heightOfWorld/2), transform.position.y + (yScale/2));
		}
		return new Vector2(x,y);
	}
	void upgradeWave(){
		if(waveNumber%3 == 0){
			if (typesOfEnnemies < ennemies.Count){
				typesOfEnnemies++;
			}
		}
		if(waveNumber%5 == 1){
			ennemyLevel++;
		}
	}
	void createWave(int typesOfEnnemies){
		for (int i = 0; i < typesOfEnnemies; i++){
			int amount = ennemies[i].GetComponent<colony>().amount;
			int maxGroupNb = ennemies[i].GetComponent<colony>().maxGroupNb;
			amount += (ennemies[i].GetComponent<colony>().amountPerLevel * ennemyLevel);
			Vector2 randPos = getRandSpawnPoint();
			for (int j = 0; j < amount; j++){
				if (j % maxGroupNb == 0 && j > 0) randPos = getRandSpawnPoint();
				createEnnemy(ennemies[i], new Vector2 (randPos.x+(j*0.1f), randPos.y+(j*0.1f)));
			}
		}
	}

	void createEnnemy(GameObject g, Vector2 pos){
		waveCount++;
		GameObject ennemy = (GameObject) Instantiate(g,pos, Quaternion.identity);
		ennemy.GetComponent<colony>().level = ennemyLevel;
		ennemy.GetComponent<colony>().center = center;
		ennemy.GetComponent<colony>().waveSpawner = gameObject;
	}
}
