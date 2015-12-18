﻿using Assets.Scripts.Tokens;
using Assets.Scripts.Timers;
using Assets.Scripts.Data;
using Assets.Scripts.Attacks;
using UnityEngine;
using TeamUtility.IO;

namespace Assets.Scripts.Player
{
    /*
     * Handles all the components related to in game Life
     * such as lives, health, respawning, etc
     */
	public class Life : ControllerObject
	{
        public const float MAX_HEALTH = 100f;

        private float health = MAX_HEALTH, lives = Mathf.Infinity;

        // Last player to hit this player
        private PlayerID lastAttacker = PlayerID.None;

        public void ModifyHealth(float delta, PlayerID id = PlayerID.None)
        {
            health = Mathf.Clamp((health + delta), 0, MAX_HEALTH);
            Debug.Log("Health: " + health);
            if (health <= 0) Die(id);
        }

        private void Die(PlayerID lastID = PlayerID.None)
        {
            lastAttacker = lastID;
            controller.Disable();
            if(--lives > 0)
            {
                Debug.Log("Lives: " + lives);
                // Tell GameManager to setup respawn
                GameManager.instance.Respawn(controller.ID);
            }
        }

        public void Respawn()
        {
            SpawnAttack[] currentAttacks = GetComponents<SpawnAttack>();
            for(int i = 0; i < currentAttacks.Length; i++)
            {
                Destroy(currentAttacks[i]);
            }

            Timer[] timers = GetComponents<Timer>();
            for(int i = 0; i < timers.Length; i++)
            {
                if (timers[i].ID.EndsWith("Attack")) Destroy(timers[i]);
            }

            controller.ArcheryComponent.ClearAllTokens();

            health = MAX_HEALTH;
            controller.Enable();
        }

        // Overriding the collect token method from player controller object
        public override void CollectToken(Token token)
        {
            // Handle what type of token was collected
            if (token.GetType().Equals(typeof(HealthToken)))
            {
                // Add health back to the player
               ModifyHealth(((HealthToken)token).Health);
            }
        }

        #region C# Properties
        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        public float Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        #endregion
    }
}
