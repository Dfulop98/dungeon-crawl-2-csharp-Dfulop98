﻿using System.Collections.Generic;
using Assets.Source.Actors.Items;
using Assets.Source.Core;
using Assets.Source.scripts;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class Player : Character
    {
        public int Score { get; set; } = 0;
        public override int Damage { get; set; } = 10;
        public override int Health { get; set; } = 100;

        public List<Item> _inventory = new List<Item>();
        public Item _floorItem = null;

        protected override void OnUpdate(float deltaTime)
        {
            HealthBar_Script.CurrentHealth = (float)Health;
            HealthBar_Script.HealthBar.fillAmount = HealthBar_Script.CurrentHealth / HealthBar_Script.MaxHealth;

            UserInterface.Singleton.SetText($"Health: {Health}\nDamage: {Damage}\nScore: {Score}", UserInterface.TextPosition.TopRight);

            UserInterface.Singleton.SetText($"{CreateInventoryString()}", UserInterface.TextPosition.BottomRight);


            if (Input.GetKeyDown(KeyCode.W))
            {
                // Move up
                TryMove(Direction.Up);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // Move down
                TryMove(Direction.Down);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // Move left
                TryMove(Direction.Left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // Move right
                TryMove(Direction.Right);
            }

            if (Input.GetKeyDown(KeyCode.E) && _floorItem != null)
            {
                _floorItem.Pickup(this);
                _floorItem = null;
            }

            //test HealthBar
            if (Input.GetKeyDown(KeyCode.Space))
            {

                Health -= Damage;
            }
        }

        public new void TryMove(Direction direction)
        {
            var vector = direction.ToVector();
            (int x, int y) targetPosition = (Position.x + vector.x, Position.y + vector.y);

            var actorAtTargetPosition = ActorManager.Singleton.GetActorAt(targetPosition);

            if (actorAtTargetPosition == null)
            {
                // No obstacle found, just move
                Position = targetPosition;
                CameraController.Singleton.Position = targetPosition;
                _floorItem = null;
            }
            else
            {
                if (actorAtTargetPosition is Item item)
                {
                    Position = targetPosition;
                    CameraController.Singleton.Position = targetPosition;

                    _floorItem = item;
                }
                else if (actorAtTargetPosition.OnCollision(this))
                {
                    // Allowed to move
                    // Position = targetPosition;
                }
            }
        }

        private string CreateInventoryString()
        {
            if (_inventory.Count == 0)
            {
                return "None";
            }
            else
            {
                string output = "";
                _inventory.ForEach(item => output += item.DefaultName);
                return output;
            }
        }

        public override bool OnCollision(Actor anotherActor)
        {
            if (anotherActor is Item item)
            {
                _floorItem = item;
            }
            else
            {
                _floorItem = null;
            }
            return false;
        }

        protected override void OnDeath()
        {
            OnDeathFeedBack();
            ActorManager.Singleton.DestroyActor(this);
        }

        protected override void OnDeath(Player player)
        {
            
        }

        protected override void OnDeathFeedBack()
        {
            Debug.Log("Oh no, I'm dead!");
        }

        public override void ApplyDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                // Die
                OnDeath();
            }
        }

        public override void ApplyDamage(int damage, Player player)
        {
            
        }

        public override int DefaultSpriteId => 24;
        public override string DefaultName => "Player";

    }
}
