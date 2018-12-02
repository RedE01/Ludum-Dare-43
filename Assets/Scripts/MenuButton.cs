using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public void ChangeScene(int number) {
		SceneManager.LoadScene(number);
	}

	public void ExitGame() {
		Application.Quit();
	}
}