using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

	private IList<Player> playerList = new List<Player>();

	private int selected = 0;

	void Start() {
	}

	public int AddToPlayerList(Player p) {
		int id = playerList.Count;

		playerList.Add(p);
		if(id == 0) {
			p.MakeSelected(true);
		}
		else {
			p.MakeSelected(false);
		}

		return id;
	}

	public void requestSelected(int id) {
		selected = id;
		for(int i = 0; i < playerList.Count; i++) {
			bool isSelected = i == id ? true : false;

			playerList[i].MakeSelected(isSelected);
		}
	}
}
