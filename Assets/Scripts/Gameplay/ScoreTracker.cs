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

	int _perfect = 0;
	int _good = 0;
	int _ok = 0;
	int _miss = 0;

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
		_perfect = 0;
		_good = 0;
		_ok = 0;
		_miss = 0;
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
		_perfect++;
	}

	void IncrementGood(){
		_good++;
	}

	void IncrementOk(){
		_ok++;
	}

	void IncrementMiss(){
		_miss++;
	}

	public int getPerfect(){
		return _perfect; 
	}
	public int getGood(){
		return _good; 
	}
	public int getOk(){
		return _ok; 
	}
	public int getMiss(){
		return _miss; 
	}
}
