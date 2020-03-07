using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour {
	static ScoreTracker _self = null;

	public static ScoreTracker instance{
		get => _self;
	}

	void Start(){
		_self = this;
		ResetCounts();
		scoreDisplay.gameObject.SetActive(false);
	}

	int perfect = 0;
	int good = 0;
	int ok = 0;
	int miss = 0;

	public ScoreUpdater scoreDisplay;

	public void StartGame(){
		SongPlayer player = SongPlayer.instance;
		player.PlaySong();
		if(!player.isPlaying){
			print("Couldn't start the game. Invalid track selection probably");
			return;
		}

		ResetCounts();
		scoreDisplay.gameObject.SetActive(true);
	}

	public void ResetCounts(){
		perfect = 0;
		good = 0;
		ok = 0;
		miss = 0;
	}

	public void Increment(TimingResult t){
		switch(t){
			case TimingResult.Perfect:{
				IncrementPerfect();
				break;
			}
			case TimingResult.Good:{
				IncrementGood();
				break;
			}
			case TimingResult.Ok:{
				IncrementOk();
				break;
			}
			case TimingResult.Miss:{
				IncrementMiss();
				break;
			}
		}
	}

	void IncrementPerfect(){
		perfect++;
	}

	void IncrementGood(){
		good++;
	}

	void IncrementOk(){
		ok++;
	}

	void IncrementMiss(){
		miss++;
	}

	public int getPerfect(){
		return perfect; 
	}
	public int getGood(){
		return good; 
	}
	public int getOk(){
		return ok; 
	}
	public int getMiss(){
		return miss; 
	}
}
