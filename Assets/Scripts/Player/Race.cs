using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Race
{
    public int max_hp;
    public int speed;
    public int jump_force;
    public int dmg;
    public int bullet_size;

    public static Race CreateRace(int hp, int speed, int jump, int dmg, int bullet_s)
    {
        Race race = new Race();
        race.max_hp = hp;
        race.speed = speed;
        race.jump_force = jump;
        race.dmg = dmg;
        race.bullet_size = bullet_s;


        return race;
    }   

    public int Max_HP { get { return max_hp; } }
    public int Speed { get { return speed; } }
    private int Jump_Force { get { return jump_force; } }
    private int Damage { get { return dmg; } }
    private int Bullet_Size { get { return bullet_size; } }
}
