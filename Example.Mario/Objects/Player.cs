using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mario.Scenes;

namespace Mario.Objects
{
    public class Player : SosEngine.Sprite, SosEngine.IConsoleCommand
    {
        protected enum MovingDirection
        {
            Left,
            Right,
        }

        public EntityManager EntityManager { get; set; }

        /// <summary>
        /// Refrence to the main play scene if player is currently in a sub play scene.
        /// E.g. when entered a pipe the main level is the MainPlayScene.
        /// </summary>
        public PlayScene MainPlayScene { get; set; }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        protected int score;

        protected const float minWalkSpeed = 0.0664f;
        protected const float maxWalkSpeed = 1.5625f;
        protected const float maxRunSpeed = 2.5625f;
        protected const float walkAcc = 1.1354f;
        protected const float runAcc = 1.139f;
        protected const float deceleration = 0.9375f;
        protected const float skiddingDeceleration = 0.9075f;
        protected const float skidTurnaroundSpeed = 0.8f;
        protected float movementSpeed = 0.0f;

        protected SosEngine.Level level;

        protected List<Pipe> pipes;

        protected bool isEnteringPipe;
        protected int enteringPipeCount;
        protected int enteringPipeMaxCount;
        protected Pipe.PipeOrientation pipeOrientation;
        protected Pipe.PipeTypes pipeType;
        protected Pipe.PipeAction pipeAction;
        protected string pipeDestination;
        protected bool isExitingPipe;
        protected int exitPipeCount;
        protected int exitPipeMaxCount;
        protected int pipeSlowerCounter;

        protected MovingDirection movingDirection;
        protected MovingDirection lastMovingDirection;

        protected bool isTiny;
        protected bool isSuper;

        protected bool isGrowing;
        protected int growCount;

        protected bool isSitting;

        protected bool isMoving;
        protected bool isActuallyMoving;
        protected bool isSkidding;
        protected bool isJumping;
        protected int jumpCount;
        protected float jumpSpeed;
        protected bool bouncedOnEnemy;
        protected int fallPositionY;
        protected float horizontalJumpSpeed;
        protected bool isFallingAfterJumping;
        protected float fallSpeed;
        protected bool isOnLadder;
        protected int ladderCenter;
        protected bool lastFalling;

        protected bool controlLeft;
        protected bool controlRight;
        protected bool controlUp;
        protected bool controlDown;
        protected bool controlRun;
        protected bool controlJump;

        protected bool oldControlRun;

        protected int animationDelay;
        protected int animationCount;
        protected int animationFrame;

        protected int fireballCooldown;
        protected bool isShooting;

        public PlayerStats Stats
        {
            get
            {
                return new PlayerStats
                {
                    Score = this.score,
                    IsTiny = this.isTiny,
                    IsSuper = this.isSuper
                };
            }
            set
            {
                this.score = value.Score;
                this.isTiny = value.IsTiny;
                this.isSuper = value.IsSuper;
            }
        }

        public Player(Game game, string spriteFrameName, SosEngine.Level level = null) :
            base(game, spriteFrameName)
        {
            this.level = level;
            this.isTiny = true;
            this.pipes = new List<Pipe>();
            Reset();
        }

        public void Reset()
        {
            isMoving = false;
            isActuallyMoving = false;
            isJumping = false;
            jumpCount = 0;
            jumpSpeed = 0;
            horizontalJumpSpeed = 0;
            isFallingAfterJumping = false;
            isOnLadder = false;
            controlLeft = false;
            controlRight = false;
            controlUp = false;
            controlDown = false;
            controlJump = false;
            movingDirection = MovingDirection.Right;
            animationDelay = 100;
            fallSpeed = 1.0f;
            isEnteringPipe = false;
            isExitingPipe = false;
            ClipBottomAmount = 0;
            ClipRightAmount = 0;
            SetSpriteFrame("mario_move_0");
        }

        public override void SetSpriteFrame(string spriteFrameName)
        {
            if (isGrowing && spriteFrameName == "mario_sitting")
            {
                spriteFrameName = "mario_move_0";
            }
            if (isTiny || isGrowing && growCount % 12 >= 6)
            {
                spriteFrameName = "tiny_" + spriteFrameName;
            } else if (isSuper)
            {
                spriteFrameName = "super_" + spriteFrameName;
            }
            base.SetSpriteFrame(spriteFrameName);
        }

        protected override Rectangle GetBoundingBox()
        {
            Rectangle rect = base.GetBoundingBox();
            if (isTiny || isSitting)
            {
                rect.Height = rect.Height - 12;
                rect.Offset(0, 12);
            }
            return rect;
        }

        /// <summary>
        /// Bounding box that can kill an enemy.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetLethalBounds()
        {
            return GetBoundingBox();
        }

        public void AddScore(int score, bool showEffect, Vector2 position)
        {
            AddScore(score, showEffect, (int)Math.Round(position.X), (int)Math.Round(position.Y));
        }

        public void AddScore(int scoreToAdd, bool showEffect, int x = 0, int y = 0)
        {
            // Score -2 = 1UP

            if (score > 0)
            {
                score += scoreToAdd;
            }
            if (showEffect)
            {
                if (scoreToAdd == -2 || scoreToAdd == 100 || scoreToAdd == 200 || scoreToAdd == 400 || scoreToAdd == 500 || scoreToAdd == 800 || scoreToAdd == 1000 || scoreToAdd == 2000 || scoreToAdd == 4000 || scoreToAdd == 8000)
                {
                    if (x == 0)
                    {
                        x = (int)Math.Round(Position.X);
                    }
                    if (y == 0)
                    {
                        y = (int)Math.Round(Position.Y);
                    }
                    EntityManager.AddEntity(new ScoreEffect(Game, x + level.GetScrollX(), y, scoreToAdd));
                }
            }
        }

        public void AddPipe(Pipe pipe)
        {
            pipes.Add(pipe);
        }

        /*
        protected void CheckPickups()
        {
            Rectangle boundingBox = GetBoundingBox();
            Rectangle pickupBox = new Rectangle(boundingBox.X + 10, boundingBox.Y + 16, boundingBox.Width - 20, boundingBox.Height - 20);
            pickupBox.Offset(-level.GetScrollX(), 0);

            List<SosEngine.Sprite> sprites = PickupManager.GetCollidingSprites(pickupBox);
            for (int i = 0; i < sprites.Count; i++)
            {
                Pickup pickup = (Pickup)sprites[i];
                SosEngine.Core.Log(string.Format("Picked up: {0}", pickup.Block));
                score += pickup.Score;
                PickupManager.PlaySoundForPickup(pickup);
                PickupManager.RemoveSprite(pickup);
                
                SosEngine.Core.SceneManager.CurrentScene.RemoveGameComponent(pickup);
                
            }
        }
        */

        /// <summary>
        /// Returns true if player is able to kill an enemy
        /// based on the players current state.
        /// </summary>
        /// <returns></returns>
        public bool CanJumpOnEnemy()
        {
            return !isJumping && !IsGrounded();
        }

        /// <summary>
        /// Handles collision with various entities, such as powerups and enemies.
        /// </summary>
        protected void HandleEntityCollision()
        {
            Rectangle boundingBox = GetBoundingBox();
            Rectangle pickupBox = new Rectangle(boundingBox.X + 10, boundingBox.Y + 16, boundingBox.Width - 20, boundingBox.Height - 20);
            pickupBox.Offset(-level.GetScrollX(), 0);

            List<SosEngine.Sprite> sprites = EntityManager.GetCollidingSprites(pickupBox);

            for (int i = 0; i < sprites.Count; i++)
            {
                BaseEntity entity = (BaseEntity)sprites[i];
                if (entity is Mushroom)
                {
                    var mushroom = (Mushroom)entity;
                    if (mushroom.MushroomType == Mushroom.MushroomTypes.Mushroom && isTiny)
                    {
                        isTiny = false;
                        isGrowing = true;
                        growCount = 0;
                        SetSpriteFrame(spriteFrame.FrameName);
                        SosEngine.Core.PlaySound("Powerup");
                        AddScore(1000, true, mushroom.TopCenter);
                    }
                    if (mushroom.MushroomType == Mushroom.MushroomTypes.OneUp)
                    {
                        AddScore(-2, true, mushroom.TopCenter);
                        SosEngine.Core.PlaySound("1up");
                    }
                    EntityManager.RemoveSprite(entity);
                }
                if (entity is Flower)
                {
                    if (isTiny)
                    {
                        isTiny = false;
                        isGrowing = true;
                        growCount = 0;
                        SetSpriteFrame(spriteFrame.FrameName);
                        SosEngine.Core.PlaySound("Powerup");
                        AddScore(1000, true, entity.TopCenter);
                    } else if (!isSuper)
                    {
                        isSuper = true;
                        isGrowing = true;
                        growCount = 0;
                        SetSpriteFrame(spriteFrame.FrameName);
                        SosEngine.Core.PlaySound("Powerup");
                        AddScore(1000, true, entity.TopCenter);
                    }
                    EntityManager.RemoveSprite(entity);
                }
                if (entity is Enemy)
                {
                    ((Enemy)entity).CollideWithPlayer(this);
                }
            }

        }

        /// <summary>
        /// Check if player can move to the left.
        /// </summary>
        /// <returns></returns>
        protected bool CanMoveLeft()
        {
            int y = (int)Math.Ceiling(Position.Y) + spriteFrame.Rectangle.Height - 1;
            int x = (int)Math.Floor(Position.X - 6) + spriteFrame.Rectangle.Width / 2;

            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y)))
            {
                return false;
            }
            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y - 8)))
            {
                return false;
            }
            if (!isTiny && !isSitting && Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y - 16)))
            {
                return false;
            }

            // Player can move 4 pixels outside of screen
            if (x < 1 - 4)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if player can move to the right.
        /// </summary>
        /// <returns></returns>
        protected bool CanMoveRight()
        {
            int y = (int)Math.Ceiling(Position.Y) + spriteFrame.Rectangle.Height - 1;
            int x = (int)Math.Ceiling(Position.X + 6) + spriteFrame.Rectangle.Width / 2;

            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y)))
            {
                return false;
            }
            if (Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y - 8)))
            {
                return false;
            }
            if (!isTiny && !isSitting && Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y - 16)))
            {
                return false;
            }

            // Player can move 4 pixels outside of screen
            if (x > level.WidthInPixels - 1 + 4)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure player is won't stuck inside level blocks.
        /// </summary>
        protected void HandleForceMovement()
        {
            int y = (int)Math.Ceiling(Position.Y) + spriteFrame.Rectangle.Height - 1;
            int x = (int)Math.Ceiling(Position.X + 4) + spriteFrame.Rectangle.Width / 2;
            if (!isTiny && !isSitting && Helpers.LevelHelper.IsWall(level.GetBlockAtPixel("Block", x, y - 16)))
            {
                Position = new Vector2(Position.X + 1, Position.Y);
            }
        }

        /// <summary>
        /// Check if player can begin to jump.
        /// </summary>
        /// <returns></returns>
        protected bool CanJump()
        {
            return IsGrounded();
        }

        /// <summary>
        /// Check if player is standing on solid ground.
        /// </summary>
        /// <param name="ignoreLadder"></param>
        /// <returns></returns>
        protected bool IsGrounded(bool ignoreLadder = false)
        {
            if (isJumping || (isOnLadder && !ignoreLadder))
            {
                return false;
            }

            int y = (int)Math.Ceiling(Position.Y) + spriteFrame.Rectangle.Height;
            if (y % 16 != 0)
            {
                return false;
            }

            int leftX = (int)Math.Floor(Position.X - 5) + spriteFrame.Rectangle.Width / 2;
            int rightX = (int)Math.Ceiling(Position.X + 5) + spriteFrame.Rectangle.Width / 2;

            if (level != null)
            {
                int block1 = level.GetBlockAtPixel("Block", leftX, y + 1);
                int block2 = level.GetBlockAtPixel("Block", rightX, y + 1);
                return Helpers.LevelHelper.IsWall(block1) || Helpers.LevelHelper.IsWall(block2);
            }

            return true;
        }

        /// <summary>
        /// Check if player can fall.
        /// </summary>
        /// <returns></returns>
        protected bool CanFall()
        {
            if (isJumping || isOnLadder)
            {
                return false;
            }
            return !IsGrounded();
        }

        /// <summary>
        /// Check if player enters a ladder.
        /// </summary>
        protected void CheckEnterLadder()
        {
            int x = (int)Position.X + spriteFrame.Rectangle.Width / 2;
            int y = (int)Position.Y + spriteFrame.Rectangle.Height;
            y = controlDown ? y + 8 + 1 : y - 1;
            if (Helpers.LevelHelper.IsLadder(level.GetBlockAtPixel("Block", x, y)))
            {
                ladderCenter = ((x / 8) * 8) - 2;
                if (Helpers.LevelHelper.IsLadder(level.GetBlockAtPixel("Block", x - 8, y)))
                {
                    ladderCenter -= 8;
                }
                isOnLadder = true;
                animationCount = animationDelay * 2;
                animationFrame = 0;
                movementSpeed = 0f;
                Position = new Vector2(Position.X, controlDown ? Position.Y + 2 : Position.Y - 2);
                isMoving = true;
                isActuallyMoving = true;
            }
        }

        /// <summary>
        /// Check if player exits a ladder.
        /// </summary>
        protected void CheckExitLadder()
        {
            if (IsGrounded(true))
            {
                isOnLadder = false;
                animationCount = animationDelay;
                animationFrame = 0;
                isMoving = true;
                isActuallyMoving = true;
            }
        }

        protected float CalculateHorizontalJumpMovement()
        {
            return 0;
            /*
            if ((isJumping || isFallingAfterJumping) && ((horizontalJumpSpeed < 0 && CanMoveLeft()) || (horizontalJumpSpeed > 0 && CanMoveRight())))
            {
                return horizontalJumpSpeed;
            }
            else
            {
                return 0;
            }
            */
        }

        /// <summary>
        /// Apply gravity to player.
        /// </summary>
        protected void ApplyGravity()
        {
            int fallCount = (int)Math.Ceiling(fallSpeed);
            if (fallCount > 3)
            {
                fallCount = 3;
            }
            for (int i = 0; i < fallCount; i++)
            {
                if (CanFall())
                {
                    if (!lastFalling)
                    {
                        if (!isFallingAfterJumping)
                        {
                            fallPositionY = (int)Position.Y;
                            float pitch = 1f - (Position.Y / 200f);
                            // SosEngine.Core.PlaySound("fall", 1.0f, pitch, 0f);
                        }
                    }
                    float horizontalMovement = i == 0 ? CalculateHorizontalJumpMovement() : 0;
                    Position = new Vector2(Position.X + horizontalMovement, Position.Y + (fallSpeed / (float)fallCount));

                    // HandleScrolling(horizontalMovement);
                    
                    fallSpeed = fallSpeed * 1.07f;
                    
                    if (fallSpeed > 3f)
                    {
                        fallSpeed = 3f;
                    }
                    lastFalling = true;
                    Rectangle rect = GetBoundingBox();
                    rect.Y += 14;
                    rect.Height = 4;
                    
                }
                else
                {
                    if (lastFalling && !isJumping)
                    {
                        
                        // Landed
                        if (Position.Y - fallPositionY > 30)
                        {
                            Die();
                        }
                    }
                    Position = new Vector2(Position.X, (float)Math.Round(Position.Y));
                    fallSpeed = 1f;
                    isFallingAfterJumping = false;
                    lastFalling = false;
                    break;
                }
            }

        }
        
        /// <summary>
        /// Apply acceleration to player.
        /// </summary>
        protected void ApplyAcceleration()
        {
            if (movementSpeed < minWalkSpeed)
            {
                movementSpeed = minWalkSpeed;
            }
            movementSpeed = movementSpeed * (controlRun ? runAcc : walkAcc);
            if (movementSpeed > (controlRun ? maxRunSpeed : maxWalkSpeed))
            {
                movementSpeed = (controlRun ? maxRunSpeed : maxWalkSpeed);
            }
        }

        /// <summary>
        /// Apply deceleration to player.
        /// </summary>
        protected void ApplyDeceleration()
        {
            if (!isActuallyMoving)
            {
                if (movementSpeed > minWalkSpeed)
                {
                    movementSpeed = movementSpeed * (isSkidding ? skiddingDeceleration : deceleration);
                    if (movingDirection == MovingDirection.Left)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                }
            }
        }

        protected void Die()
        {
        }

        /// <summary>
        /// Move player to the left if possible.
        /// </summary>
        protected void MoveLeft()
        {
            if (CanMoveLeft())
            {
                movingDirection = MovingDirection.Left;

                isMoving = true;

                var movement = movementSpeed;
                while (movement > 1 && CanMoveLeft())
                {
                    Position = new Vector2(Position.X - 1, Position.Y);
                    movement = movement - 1;
                    isActuallyMoving = true;
                }
                if (CanMoveLeft())
                {
                    Position = new Vector2(Position.X - movement, Position.Y);
                    isActuallyMoving = true;
                }

                HandleScrollingLeft();
            }
        }

        /// <summary>
        /// Move player to the right if possible.
        /// </summary>
        protected void MoveRight()
        {
            if (CanMoveRight())
            {
                movingDirection = MovingDirection.Right;

                isMoving = true;

                var movement = movementSpeed;
                while (movement > 1 && CanMoveRight())
                {
                    Position = new Vector2(Position.X + 1, Position.Y);
                    movement = movement - 1;
                    isActuallyMoving = true;
                }
                if (CanMoveRight())
                {
                    Position = new Vector2(Position.X + movement, Position.Y);
                    isActuallyMoving = true;
                }

                HandleScrollingRight();
            }
        }

        /// <summary>
        /// Handles scrolling of level.
        /// </summary>
        /// <param name="xMovement"></param>
        protected void HandleScrolling(float xMovement)
        {
            if (xMovement < 0)
            {
                HandleScrollingLeft();
            }
            else if (xMovement > 0)
            {
                HandleScrollingRight();
            }
        }

        /// <summary>
        /// Handle left scrolling of the level.
        /// </summary>
        protected void HandleScrollingLeft()
        {
            if (Position.X < SosEngine.Core.RenderWidth * 0.35f)
            {
                float scrollAmount = Position.X - (SosEngine.Core.RenderWidth * 0.35f);
                if (level.ScrollX(-scrollAmount))
                {
                    Position = new Vector2(Position.X - scrollAmount, Position.Y);
                }
            }
        }

        /// <summary>
        /// Handle right scrolling of the level.
        /// </summary>
        protected void HandleScrollingRight()
        {
            if (Position.X > SosEngine.Core.RenderWidth * 0.65f)
            {
                float scrollAmount = Position.X - (SosEngine.Core.RenderWidth * 0.65f);
                if (level.ScrollX(-scrollAmount))
                {
                    Position = new Vector2(Position.X - scrollAmount, Position.Y);
                }
            }
        }

        protected float GetAdjustmentForLadderCenter()
        {
            // Just return 0 as scrolling will mess this up!
            return 0;
            /*
            if (Position.X < ladderCenter)
            {
                float d = ladderCenter - Position.X;
                if (d > 0.5f)
                {
                    d = 0.5f;
                }
                return d;
            }
            else if (Position.X > ladderCenter)
            {
                float d = Position.X - ladderCenter;
                if (d > 0.5f)
                {
                    d = 0.5f;
                }
                return -d;
            }
            return 0;
            */
        }

        protected bool AtEndOfOpenEndedLadder()
        {
            int x = (int)Position.X + spriteFrame.Rectangle.Width / 2;
            int y = (int)Position.Y + spriteFrame.Rectangle.Height;
            y = y - 1;
            return (level.GetBlockAtPixel("Block", x, y) == 0);
        }

        protected void MoveUp()
        {
            if (!AtEndOfOpenEndedLadder())
            {
                isMoving = true;
                isActuallyMoving = true;
                var adjustment = GetAdjustmentForLadderCenter();
                Position = new Vector2(Position.X + adjustment, Position.Y - 1);
                HandleScrolling(adjustment);
            }
        }

        protected void MoveDown()
        {
            isMoving = true;
            isActuallyMoving = true;
            var adjustment = GetAdjustmentForLadderCenter();
            Position = new Vector2(Position.X + adjustment, Position.Y + 1);
            HandleScrolling(adjustment);
        }

        public void BounceOnEnemy()
        {
            BeginJump(true);
        }

        /// <summary>
        /// Begin to jump.
        /// </summary>
        /// <param name="bouncedOnEnemy"></param>
        protected void BeginJump(bool bouncedOnEnemy = false)
        {
            SosEngine.Core.Log("Mario jumped at time: " + DateTime.Now.ToLongTimeString());

            this.bouncedOnEnemy = bouncedOnEnemy;

            fallPositionY = (int)Position.Y;
            jumpSpeed = 4.0f;
            isJumping = true;
            jumpCount = 0;

            isSkidding = false;

            horizontalJumpSpeed = controlLeft ? -movementSpeed : controlRight ? movementSpeed : 0;

            /*
            if (controlLeft || controlRight)
            {
                if (isSkidding)
                {
                    movingDirection = controlRight ? MovingDirection.Left : MovingDirection.Right;
                    movementSpeed = minWalkSpeed;
                } 
                else
                {
                    movingDirection = controlLeft ? MovingDirection.Left : MovingDirection.Right;
                }
            }
            */
            SetSpriteFrame(string.Format("mario_jump", System.Enum.GetName(typeof(MovingDirection), movingDirection).ToLower()));
            SosEngine.Core.PlaySound("jump");
        }

        /// <summary>
        /// Handles block hitting.
        /// </summary>
        /// <returns></returns>
        protected bool HandleHitBlock()
        {
            var stopJumping = false;

            int x = BoundingBox.X + (BoundingBox.Width / 2);
            int y = BoundingBox.Y + 4;
            var bx = 0;
            var by = 0;
            var block = level.GetBlockAtPixel("Block", x - 2, y, out bx, out by);
            if (block == 0)
            {
                block = level.GetBlockAtPixel("Block", x + 2, y, out bx, out by);
            }

            var item = level.GetBlock("Items", bx, by);
            
            if (Helpers.LevelHelper.IsWall(block))
            {
                if (Helpers.LevelHelper.IsQuestionBlock(block))
                {
                    level.StopTileAnimationAt("Block", bx, by);
                    level.PutBlock("Block", bx, by, 67);
                    level.BounceBlock("Block", bx, by);
                }
                else if (Helpers.LevelHelper.IsBreakableBlock(block))
                {
                    if (item != 0)
                    {
                        level.PutBlock("Block", bx, by, 67);
                        level.BounceBlock("Block", bx, by);
                    }
                    else
                    {
                        if (isTiny)
                        {
                            level.BounceBlock("Block", bx, by);
                        }
                        else
                        {
                            level.RemoveBlock("Block", bx, by);
                            EntityManager.AddEntity(new BreakBlockEffect(this.Game, "break_block_1", (bx * 16), (by * 16), BreakBlockEffect.Placement.UpperLeft));
                            EntityManager.AddEntity(new BreakBlockEffect(this.Game, "break_block_1", (bx * 16), (by * 16), BreakBlockEffect.Placement.UpperRight));
                            EntityManager.AddEntity(new BreakBlockEffect(this.Game, "break_block_1", (bx * 16), (by * 16), BreakBlockEffect.Placement.LowerLeft));
                            EntityManager.AddEntity(new BreakBlockEffect(this.Game, "break_block_1", (bx * 16), (by * 16), BreakBlockEffect.Placement.LowerRight));
                            SosEngine.Core.PlaySound("BrickShatter");
                        }
                    }
                }
                stopJumping = true;
            }

            if (item > 0)
            {
                level.RemoveBlock("Items", bx, by);
                switch (item)
                {
                    case 1:
                        if (isTiny)
                        {
                            EntityManager.AddEntity(new Mushroom(this.Game, "mushroom", Mushroom.MushroomTypes.Mushroom, bx * 16, (by * 16), level));
                        } else
                        {
                            EntityManager.AddEntity(new Flower(this.Game, bx * 16, (by * 16), level));
                        }
                        break;
                    case 2:
                        EntityManager.AddEntity(new Mushroom(this.Game, "1up_mushroom", Mushroom.MushroomTypes.OneUp, bx * 16, (by * 16), level));
                        break;
                    case 3:
                        EntityManager.AddEntity(new Mushroom(this.Game, "deadly_mushroom", Mushroom.MushroomTypes.Deadly, bx * 16, (by * 16), level));
                        break;
                    case 4:

                        EntityManager.AddEntity(new CoinEffect(this.Game, bx * 16, (by - 1) * 16));
                        AddScore(200, true, (bx * 16) + 8, (by - 1) * 16);
                        level.PutBlock("Block", bx, by, 67);
                        level.BounceBlock("Block", bx, by);
                        break;
                }

                if (block == 0)
                {
                    level.PutBlock("Block", bx, by, 67);
                    level.BounceBlock("Block", bx, by);
                }

                stopJumping = true;
            }


            return stopJumping;
        }

        /// <summary>
        /// Handles jumping.
        /// </summary>
        protected void HandleJump()
        {
            isSkidding = false;
            var horizontalMovement = CalculateHorizontalJumpMovement();
            Position = new Vector2(Position.X + horizontalMovement, Position.Y - jumpSpeed);
            HandleScrolling(horizontalMovement);
            jumpSpeed = jumpSpeed * 0.963f;
            jumpCount++;
            if (jumpCount > 26 || (!controlJump && jumpCount > (bouncedOnEnemy ? 8 : 5)) || HandleHitBlock())
            {
                Position = new Vector2(Position.X, (float)Math.Round(Position.Y));
                isJumping = false;
                isFallingAfterJumping = true;
            }
        }
        
        /// <summary>
        /// Set states of inputs controlling the player.
        /// </summary>
        /// <param name="controlLeft"></param>
        /// <param name="controlRight"></param>
        /// <param name="controlUp"></param>
        /// <param name="controlDown"></param>
        /// <param name="controlRun"></param>
        /// <param name="controlJump"></param>
        public void SetControls(bool controlLeft, bool controlRight, bool controlUp, bool controlDown, bool controlRun, bool controlJump)
        {
            this.oldControlRun = this.controlRun;
            this.controlLeft = controlLeft;
            this.controlRight = controlRight;
            this.controlUp = controlUp;
            this.controlDown = controlDown;
            this.controlRun = controlRun;
            this.controlJump = controlJump;
        }

        protected bool IsFireButtonPressed()
        {
            return (controlRun && !oldControlRun);
        }

        /// <summary>
        /// Update player animation based on players actions.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Animate(GameTime gameTime)
        {
            if (isShooting && !isTiny)
            {
                SetSpriteFrame(string.Format("mario_shooting", animationFrame.ToString()));
                return;
            }

            if (isSitting && !isTiny)
            {
                SetSpriteFrame(string.Format("mario_sitting", animationFrame.ToString()));
                return;
            }

            if (isJumping || isFallingAfterJumping)
            {
                SetSpriteFrame(string.Format("mario_jump", animationFrame.ToString()));
                return;
            }

            if (isSkidding)
            {
                SetSpriteFrame(string.Format("mario_skidding", animationFrame.ToString()));
                return;
            }

            if (isOnLadder)
            {
                if (isMoving)
                {
                    animationCount += gameTime.ElapsedGameTime.Milliseconds;
                    if (animationCount >= (animationDelay * 2))
                    {
                        animationCount = 0;
                        animationFrame++;
                        if (animationFrame > 1)
                        {
                            animationFrame = 0;
                        }
                        //SetSpriteFrame(string.Format("henry_climb_{0}", animationFrame.ToString()));
                    }
                }
            }
            else if (isMoving || controlLeft || controlRight)
            {
                animationCount += gameTime.ElapsedGameTime.Milliseconds;
                if (animationCount >= animationDelay || movingDirection != lastMovingDirection)
                {
                    animationCount = 0;
                    animationFrame++;
                    if (animationFrame > 2)
                    {
                        animationFrame = 0;
                    }
                    SetSpriteFrame(string.Format("mario_move_{0}", animationFrame.ToString()));
                }
                lastMovingDirection = movingDirection;
            }
            else
            {
                SetSpriteFrame("mario_move_0");
            }
            Flipped = movingDirection == MovingDirection.Left;
        }

        /// <summary>
        /// Check if player enters a pipe.
        /// </summary>
        protected void CheckEnterPipe()
        {
            if (IsGrounded() && !isEnteringPipe && (controlDown || controlRight))
            {
                int y = (int)Math.Ceiling(Position.Y) + spriteFrame.Rectangle.Height;
                int leftX = (int)Math.Floor(Position.X - 5) + spriteFrame.Rectangle.Width / 2;
                int rightX = (int)Math.Ceiling(Position.X + 5) + spriteFrame.Rectangle.Width / 2;
                foreach (var pipe in pipes)
                {
                    if (pipe.PipeType == Pipe.PipeTypes.Entrance)
                    {
                        if ((pipe.Orientation == Pipe.PipeOrientation.Horizontal && controlDown && pipe.Contains(leftX, y) && pipe.Contains(rightX, y)) ||
                            (pipe.Orientation == Pipe.PipeOrientation.Vertical && controlRight && pipe.Contains(rightX + 1, y - 8)))
                        {
                            pipeType = pipe.PipeType;
                            pipeOrientation = pipe.Orientation;
                            pipeDestination = pipe.Destination;
                            pipeAction = pipe.Action;
                            enteringPipeCount = 0;
                            pipeSlowerCounter = 0;
                            if (pipeOrientation == Pipe.PipeOrientation.Horizontal)
                            {
                                enteringPipeMaxCount = isTiny ? 24 : 32;
                            }
                            else
                            {
                                Position = new Vector2(pipe.BoundingBox.X - 23, Position.Y);
                                enteringPipeMaxCount = 32;
                            }
                            isEnteringPipe = true;
                            SosEngine.Core.PlaySound("PowerDown");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Begins to exit a pipe.
        /// </summary>
        /// <param name="name"></param>
        public void BeginExitPipe(string name)
        {
            var pipe = pipes.FirstOrDefault(x => x.PipeType == Pipe.PipeTypes.Exit && x.Destination.ToUpper() == name.ToUpper());
            if (pipe != null)
            {
                isSitting = false;
                movementSpeed = 0;
                movingDirection = MovingDirection.Right;
                Flipped = false;
                SetSpriteFrame("mario_move_0");

                var x = (int)pipe.BoundingBox.X - 80 - level.GetScrollX();
                level.SetOffset(-x, 0);

                if (isTiny)
                {
                    Position = new Vector2(80, pipe.BoundingBox.Y - 8);
                } 
                else
                {
                    Position = new Vector2(80, pipe.BoundingBox.Y);
                }
                
                HandleScrollingLeft();
                EntityManager.SetDrawOffset(level.GetScrollX());

                Visible = true;
                isExitingPipe = true;
                exitPipeMaxCount = isTiny ? 24 : 32;
                ClipBottomAmount = 32;
                exitPipeCount = 0;
                pipeSlowerCounter = 0;
            }
        }

        /// <summary>
        /// Handles entering a pipe.
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleEnteringPipe(GameTime gameTime)
        {
            pipeSlowerCounter++;
            if (pipeSlowerCounter % 2 == 1)
            {
                return;
            }
            enteringPipeCount++;
            if (pipeOrientation == Pipe.PipeOrientation.Horizontal)
            {
                Position = new Vector2(Position.X, Position.Y + 1);
                ClipBottomAmount = enteringPipeCount;
            }
            else
            {
                Position = new Vector2(Position.X + 1, Position.Y);
                ClipRightAmount = 9 + enteringPipeCount;
                isMoving = true;
                animationDelay = 80;
                Animate(gameTime);
            }
            if (enteringPipeCount > enteringPipeMaxCount)
            {
                isEnteringPipe = false;
                ClipBottomAmount = 0;
                ClipRightAmount = 0;
                Visible = false;
                Position = new Vector2(Position.X - 60, Position.Y);
                if (pipeAction == Pipe.PipeAction.Goto)
                {
                    SosEngine.Core.SceneManager.PushScene(new Scenes.PlayScene(Game, pipeDestination, Stats, (PlayScene)SosEngine.Core.SceneManager.CurrentScene));
                }
                else if (pipeAction == Pipe.PipeAction.Return)
                {
                    MainPlayScene.Player.Stats = Stats;
                    MainPlayScene.Player.BeginExitPipe(pipeDestination);
                    SosEngine.Core.SceneManager.PopScene(SosEngine.SceneManager.TransitionEffect.Fade, 1000);
                }
            }
        }

        /// <summary>
        /// Handles exiting a pipe.
        /// </summary>
        public void HandleExitingPipe()
        {
            pipeSlowerCounter++;
            if (pipeSlowerCounter % 2 == 1)
            {
                return;
            }
            if (exitPipeCount == 0)
            {
                SosEngine.Core.Log("Playing sound when exiting pipe!");
                SosEngine.Core.PlaySound("PowerDown");
            }
            exitPipeCount++;
            Position = new Vector2(Position.X, Position.Y - 1);
            ClipBottomAmount = exitPipeMaxCount - exitPipeCount;
            if (exitPipeCount >= exitPipeMaxCount)
            {
                isExitingPipe = false;
                ClipBottomAmount = 0;
                ClipRightAmount = 0;
            }
        }

        protected void ShootFireball()
        {
            fireballCooldown = 8;
            isShooting = true;
            var x = (int)Position.X - level.GetScrollX() + 16 - (movingDirection == MovingDirection.Left ? 4 : 0);
            var y = (int)Position.Y;
            Vector2 speed;
            if (movingDirection == MovingDirection.Left)
            {
                speed = new Vector2(-3f, 0);
            } else
            {
                speed = new Vector2(3f, 0);
            }
            EntityManager.AddEntity(new Fireball(Game, x, y, speed, level));
            SosEngine.Core.PlaySound("Fireball");
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            isMoving = false;
            isActuallyMoving = false;
            isSkidding = false;
            isShooting = false;

            if (isSitting && !isJumping)
            {
                controlLeft = false;
                controlRight = false;
            }

            if (isEnteringPipe)
            {
                HandleEnteringPipe(gameTime);
                return;
            }
            if (isExitingPipe)
            {
                HandleExitingPipe();
                return;
            }

            if (isGrowing)
            {
                isSitting = false;
                growCount++;
                if (growCount > 60)
                {
                    isGrowing = false;
                }
                SetSpriteFrame(spriteFrame.FrameName.Replace("tiny_", "").Replace("super_", ""));
                return;
            }

            if (isOnLadder)
            {
                if (controlUp)
                {
                    MoveUp();
                }
                else if (controlDown)
                {
                    MoveDown();
                }
                if (isMoving)
                {
                    CheckExitLadder();
                }
            }
            else
            {
                if (controlJump && CanJump())
                {
                    BeginJump();
                }
                if (controlLeft && CanMoveLeft())
                {
                    if (!isJumping && !isFallingAfterJumping)
                    {
                        if (movingDirection == MovingDirection.Right)
                        {
                            if (movementSpeed > skidTurnaroundSpeed)
                            {
                                isSkidding = true;
                            }
                        }
                    }
                    if (!isSkidding)
                    {
                        if ((isJumping || isFallingAfterJumping) && movingDirection == MovingDirection.Right && movementSpeed > minWalkSpeed)
                        {
                            movementSpeed = movementSpeed * skiddingDeceleration;
                        }
                        else
                        {
                            movingDirection = MovingDirection.Left;
                            ApplyAcceleration();
                            MoveLeft();
                        }
                    }
                }
                else if (controlRight && CanMoveRight())
                {
                    if (!isJumping && !isFallingAfterJumping)
                    {
                        if (movingDirection == MovingDirection.Left)
                        {
                            if (movementSpeed > skidTurnaroundSpeed)
                            {
                                isSkidding = true;
                            }
                        }
                    }
                    if (!isSkidding)
                    {
                        if ((isJumping || isFallingAfterJumping) && movingDirection == MovingDirection.Left && movementSpeed > minWalkSpeed)
                        {
                            movementSpeed = movementSpeed * skiddingDeceleration;
                        }
                        else
                        {
                            movingDirection = MovingDirection.Right;
                            ApplyAcceleration();
                            MoveRight();
                        }
                    }
                }
                else if ((controlUp || controlDown) && IsGrounded())
                {
                    CheckEnterLadder();
                }

                ApplyGravity();
                ApplyDeceleration();

                if (Position.Y + BoundingBox.Height > SosEngine.Core.RenderHeight)
                {
                    Position = new Vector2(Position.X, 0); 
                }

                if (isJumping)
                {
                    HandleJump();
                }
            }

            if (controlDown && !isJumping && !isTiny && IsGrounded())
            {
                isSitting = true;
            }
            if ((isSitting && !controlDown) || isTiny)
            {
                isSitting = false;
            }

            HandleForceMovement();

            HandleEntityCollision();
            
            CheckEnterPipe();

            if (fireballCooldown > 0)
            {
                fireballCooldown--;
            }
            if (isSuper && IsFireButtonPressed() && fireballCooldown <= 0)
            {
                ShootFireball();
            }
            if (fireballCooldown > 5)
            {
                isShooting = true;
            }

            animationDelay = 100 - (int)Math.Round(movementSpeed * 20);

            Animate(gameTime);
            base.Update(gameTime);
        }

        public void ConsoleExecute(string command, params string[] args)
        {
            if (command == "super")
            {
                isTiny = false;
                isSuper = true;
            }
            if (command == "jump")
            {
                BeginJump();
            }
            if (command == "pipe" && args.Length > 0)
            {
                BeginExitPipe(args[0]);
            }
        }
    }
}
