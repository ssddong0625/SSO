using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAssets.Scripts.Player
{
   
    public class Player : MonoBehaviour
    {
        
        private int hp;

        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;

            }
        }


        public void Awake()
        {
            hp = 100;

            
        }




    }

}

