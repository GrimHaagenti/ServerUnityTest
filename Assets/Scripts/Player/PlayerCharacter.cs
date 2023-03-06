using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter 
{
    private int id_character = -1;
    private string character_name = "";
    private Race chara_race;
    private int owner_id = -1;


    public static PlayerCharacter CreateCharacter(int id, string name, Race race, int owner)
    {
        PlayerCharacter newChara = new PlayerCharacter();
        newChara.id_character = id;
        newChara.character_name = name;
        newChara.chara_race = race;
        newChara.owner_id = owner;
        return newChara;
    }

    public static Character InstantiateCharacter()
    {
        return new Character();
    }
}
