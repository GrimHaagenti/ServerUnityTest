using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyUIManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] raceSprites;

    private int currentSelectedRace = 0;

    [SerializeField]
        private Button LeftCharaArrow;
    [SerializeField]
        private Button RightCharaArrow;
    [SerializeField]
        private SpriteRenderer CharaSprite;
    [SerializeField]
        private TMP_Text CharacterName;

    [SerializeField]
    private TMP_Text hp_text;
    [SerializeField]
    private TMP_Text speed_text;

    [SerializeField]
    private TMP_Text jump_text;

    [SerializeField]
    private TMP_Text damage_text;

    [SerializeField]
    private TMP_Text bullet_size;

    private void Start()
    {
        LeftCharaArrow.onClick.AddListener(GoLeftRace);
        RightCharaArrow.onClick.AddListener(GoRightRace);
        ChangeRace(currentSelectedRace);
    }


    private void ChangeRace(int raceid)
    {

        CharaSprite.sprite = raceSprites[raceid];
        CharacterName.text = ((RacesNames)raceid).ToString();

        hp_text.text = GameManager._GAME_MANAGER._GAME_DATA.races[raceid].Max_HP.ToString();
        speed_text.text = GameManager._GAME_MANAGER._GAME_DATA.races[raceid].speed.ToString();
        jump_text.text = GameManager._GAME_MANAGER._GAME_DATA.races[raceid].jump_force.ToString();
        damage_text.text = GameManager._GAME_MANAGER._GAME_DATA.races[raceid].dmg.ToString();
        bullet_size.text = GameManager._GAME_MANAGER._GAME_DATA.races[raceid].bullet_size.ToString();
        GameManager._GAME_MANAGER.currentRace = raceid;
    }

    private void GoLeftRace()
    {
        currentSelectedRace--;

        if(currentSelectedRace< 0)
        {
            currentSelectedRace = GameManager._GAME_MANAGER._GAME_DATA.races.Count - 1;
        }

        ChangeRace(currentSelectedRace);


    }
    private void GoRightRace()
    {
        currentSelectedRace++;

        if(currentSelectedRace> GameManager._GAME_MANAGER._GAME_DATA.races.Count - 1)
        {
            currentSelectedRace = 0;
        }

        ChangeRace(currentSelectedRace);
    }

    
}
