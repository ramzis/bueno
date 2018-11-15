﻿namespace Tadget
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerView : MonoBehaviour {

        public string playerName { get; private set; }
        public GameObject mainWindow;
        public Button bDraw;
        private float timePerTurn;
        private Coroutine coroutineView;
        
        public static PlayerView Create(string playerName, float timePerTurn)
        {
            GameObject go = new GameObject(string.Format("PlayerView_{0}", playerName));
            return go.AddComponent<PlayerView>().Init(playerName, timePerTurn);
        }

        private PlayerView Init(string playerName, float timePerTurn)
        {
            this.playerName = playerName;
            this.timePerTurn = timePerTurn;
            this.mainWindow = GameObject.Find("Panel").gameObject;

            return this;
        }

        public void EnableView()
        {
            coroutineView = StartCoroutine(EnableTimeLimitedSelection());
        }

        public void DisableView()
        {
            if(coroutineView != null)
            {
                StopCoroutine(coroutineView);
                coroutineView = null;
            }
            mainWindow.SetActive(false);
        }

        private IEnumerator EnableTimeLimitedSelection()
        {
            ToggleWindow(true);
            Debug.LogFormat("[PLAYER VIEW] Enabled Player {0} view", playerName);
            yield return new WaitForSeconds(timePerTurn);
            ToggleWindow(false);
            Debug.LogFormat("[PLAYER VIEW] Disabled Player {0} view", playerName);
            yield return null;
        }

        private void ToggleWindow(bool state)
        {
            if(mainWindow)
            {
                mainWindow.SetActive(state);
            }
        }
    }
}
