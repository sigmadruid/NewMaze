using UnityEngine;
using System.Collections;
	
public class GUIControls : MonoBehaviour 
{
	RPGCharacterController rpgCharacterController;
	[HideInInspector]
	public bool blockGui;

	void Start()
	{
		rpgCharacterController = GetComponent<RPGCharacterController>();
	}

	void OnGUI()
	{
		if(!rpgCharacterController.isDead)
		{
			if(rpgCharacterController.weapon == Weapon.RELAX || rpgCharacterController.weapon != Weapon.UNARMED)
			{
				if(GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed"))
				{
					rpgCharacterController.animator.SetBool("Relax", false);
					rpgCharacterController.isRelax = false;
					StartCoroutine(rpgCharacterController._SwitchWeapon(0));
					rpgCharacterController.weapon = Weapon.UNARMED;
					rpgCharacterController.canAction = true;
					rpgCharacterController.animator.SetTrigger("RelaxTrigger");
				}
				if(!rpgCharacterController.isSitting && !rpgCharacterController.isMoving && rpgCharacterController.weapon == Weapon.RELAX)
				{
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Sit"))
					{
						rpgCharacterController.canAction = false;
						rpgCharacterController.isSitting = true;
						rpgCharacterController.canMove = false;
						rpgCharacterController.animator.SetTrigger("Sit-SitdownTrigger");
					}
				}
				if(rpgCharacterController.isSitting && !rpgCharacterController.isMoving && rpgCharacterController.weapon == Weapon.RELAX)
				{
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Stand"))
					{
						rpgCharacterController.canAction = false;
						rpgCharacterController.isSitting = false;
						rpgCharacterController.animator.SetTrigger("Sit-StandupTrigger");
						rpgCharacterController.canMove = true;
					}
				}
			}
			if(rpgCharacterController.canAction && !rpgCharacterController.isRelax)
			{
				if(rpgCharacterController.isGrounded)
				{
					//bow and crossbow can't block
					if(rpgCharacterController.weapon != Weapon.TWOHANDBOW && rpgCharacterController.weapon != Weapon.TWOHANDCROSSBOW)
					{
						//if character is not blocking
						blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
					}
					//get hit
					if(blockGui)
					{
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
						{
							StartCoroutine(rpgCharacterController._BlockHitReact());
						}
						if(GUI.Button(new Rect(30, 270, 100, 30), "Block Break"))
						{
							StartCoroutine(rpgCharacterController._BlockBreak());
						}
					} 
					else if(!rpgCharacterController.isBlocking)
					{
						if(!rpgCharacterController.isBlocking)
						{
							if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward"))
							{
								rpgCharacterController.targetDashDirection = transform.forward;
								StartCoroutine(rpgCharacterController._Roll(1));
							}
							if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward"))
							{
								rpgCharacterController.targetDashDirection = -transform.forward;
								StartCoroutine(rpgCharacterController._Roll(3));
							}
							if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left"))
							{
								rpgCharacterController.targetDashDirection = -transform.right;
								StartCoroutine(rpgCharacterController._Roll(4));
							}
							if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right"))
							{
								rpgCharacterController.targetDashDirection = transform.right;
								StartCoroutine(rpgCharacterController._Roll(2));
							}
							//ATTACK LEFT
							if(rpgCharacterController.weapon == Weapon.RIFLE || rpgCharacterController.weapon != Weapon.ARMED || (rpgCharacterController.weapon == Weapon.ARMED && rpgCharacterController.leftWeapon != 0) && rpgCharacterController.leftWeapon != 7)
							{
								if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L"))
								{
									rpgCharacterController.Attack(1);
								}
							}
							//ATTACK RIGHT
							if(rpgCharacterController.weapon == Weapon.RIFLE || rpgCharacterController.weapon != Weapon.ARMED || (rpgCharacterController.weapon == Weapon.ARMED && rpgCharacterController.rightWeapon != 0))
							{
								if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R"))
								{
									rpgCharacterController.Attack(2);
								}
							}
							//ATTACK DUAL
							if(rpgCharacterController.leftWeapon > 7 && rpgCharacterController.rightWeapon > 7 && rpgCharacterController.leftWeapon != 14)
							{
								if(rpgCharacterController.rightWeapon != 15)
								{
									if((rpgCharacterController.leftWeapon != 16 && rpgCharacterController.rightWeapon != 17)) 
									{
										if(GUI.Button (new Rect (235, 85, 100, 30), "Attack Dual")) 
										{
											rpgCharacterController.Attack (3);
										}
									}
									else if((rpgCharacterController.leftWeapon == 16 && rpgCharacterController.rightWeapon == 17)) 
									{
										if(GUI.Button (new Rect (235, 85, 100, 30), "Attack Dual")) 
										{
											rpgCharacterController.Attack (3);
										}
									}
								}
							}
							if(GUI.Button(new Rect(25, 115, 100, 30), "Left Kick"))
							{
								rpgCharacterController.AttackKick(1);
							}
							if(GUI.Button(new Rect(130, 115, 100, 30), "Right Kick"))
							{
								rpgCharacterController.AttackKick(2);
							}
							if(rpgCharacterController.weapon == Weapon.ARMED || rpgCharacterController.weapon == Weapon.UNARMED || rpgCharacterController.weapon == Weapon.STAFF)
							{
								if(GUI.Button(new Rect(25, 330, 100, 30), "Cast Atk Left"))
								{
									rpgCharacterController.CastAttack(1);
								}
								if(rpgCharacterController.weapon != Weapon.STAFF)
								{
									if(GUI.Button(new Rect(130, 330, 100, 30), "Cast Atk Right"))
									{
										rpgCharacterController.CastAttack(2);
									}
									if(GUI.Button(new Rect(80, 365, 100, 30), "Cast Dual"))
									{
										rpgCharacterController.CastAttack(3);
									}
								}
							}
							if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
							{
								rpgCharacterController.GetHit();
							}
							if(rpgCharacterController.weapon == Weapon.UNARMED && !rpgCharacterController.isMoving)
							{
								if(GUI.Button(new Rect(1115, 310, 100, 30), "Relax"))
								{
									rpgCharacterController.animator.SetBool("Relax", true);
									rpgCharacterController.isRelax = true;
									rpgCharacterController.weapon = Weapon.RELAX;
									rpgCharacterController.canAction = false;
									rpgCharacterController.animator.SetTrigger("RelaxTrigger");
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDSWORD)
							{
								if(GUI.Button(new Rect(1115, 350, 100, 30), "2 Hand Sword"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(1));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDCLUB)
							{
								if(GUI.Button(new Rect(1000, 350, 100, 30), "2 Hand Club"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(20));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDSPEAR)
							{
								if(GUI.Button(new Rect(1115, 380, 100, 30), "2 Hand Spear"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(2));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDAXE)
							{
								if(GUI.Button(new Rect(1115, 410, 100, 30), "2 Hand Axe"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(3));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDBOW)
							{
								if(GUI.Button(new Rect(1115, 440, 100, 30), "2 Hand Bow"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(4));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDCROSSBOW)
							{
								if(GUI.Button(new Rect(1115, 470, 100, 30), "Crossbow"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(5));
								}
							}
							if(rpgCharacterController.weapon != Weapon.RIFLE)
							{
								if(GUI.Button(new Rect(1000, 470, 100, 30), "Rifle"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(18));
								}
							}
							if(rpgCharacterController.weapon != Weapon.STAFF)
							{
								if(GUI.Button(new Rect(1115, 500, 100, 30), "Staff"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(6));
								}
							}
							if(rpgCharacterController.leftWeapon != 7)
							{
								if(GUI.Button(new Rect(1115, 700, 100, 30), "Shield"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(7));
								}
							}
							if(rpgCharacterController.leftWeapon != 8)
							{
								if(GUI.Button(new Rect(1065, 540, 100, 30), "Left Sword"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(8));
								}
							}
							if(rpgCharacterController.rightWeapon != 9)
							{
								if(GUI.Button(new Rect(1165, 540, 100, 30), "Right Sword"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(9));
								}
							}
							if(rpgCharacterController.leftWeapon != 10)
							{
								if(GUI.Button(new Rect(1065, 570, 100, 30), "Left Mace"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(10));
								}
							}
							if(rpgCharacterController.rightWeapon != 11)
							{
								if(GUI.Button(new Rect(1165, 570, 100, 30), "Right Mace"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(11));
								}
							}
							if(rpgCharacterController.leftWeapon != 12)
							{
								if(GUI.Button(new Rect(1065, 600, 100, 30), "Left Dagger"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(12));
								}
							}
							if(rpgCharacterController.leftWeapon != 13)
							{
								if(GUI.Button(new Rect(1165, 600, 100, 30), "Right Dagger"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(13));
								}
							}
							if(rpgCharacterController.leftWeapon != 14)
							{
								if(GUI.Button(new Rect(1065, 630, 100, 30), "Left Item"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(14));
								}
							}
							if(rpgCharacterController.leftWeapon != 15)
							{
								if(GUI.Button(new Rect(1165, 630, 100, 30), "Right Item"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(15));
								}
							}
							if(rpgCharacterController.leftWeapon != 16)
							{
								if(GUI.Button(new Rect(1065, 660, 100, 30), "Left Pistol"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(16));
								}
							}
							if(rpgCharacterController.leftWeapon != 17)
							{
								if(GUI.Button(new Rect(1165, 660, 100, 30), "Right Pistol"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(17));
								}
							}
							if(rpgCharacterController.rightWeapon != 19)
							{
								if(GUI.Button(new Rect(1000, 380, 100, 30), "1 Hand Spear"))
								{
									StartCoroutine(rpgCharacterController._SwitchWeapon(19));
								}
							}

						}
					}
				}
				if(rpgCharacterController.canJump || rpgCharacterController.canDoubleJump)
				{
					if(rpgCharacterController.isGrounded)
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Jump"))
						{
							if(rpgCharacterController.canJump && rpgCharacterController.isGrounded)
							{
								StartCoroutine(rpgCharacterController._Jump());
							}
						}
					} 
					else if(rpgCharacterController.rpgCharacterState != RPGCharacterState.CLIMBING)
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Double Jump"))
						{
							if(rpgCharacterController.canDoubleJump && !rpgCharacterController.isDoubleJumping)
							{
								StartCoroutine(rpgCharacterController._Jump());
							}
						}
					}
				}
				if(!blockGui && !rpgCharacterController.isBlocking && rpgCharacterController.isGrounded)
				{
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death"))
					{
						StartCoroutine(rpgCharacterController._Death());
					}
				}
				//Climbing
				if(!blockGui && !rpgCharacterController.isBlocking && rpgCharacterController.isGrounded && rpgCharacterController.rpgCharacterState != RPGCharacterState.CLIMBING && rpgCharacterController.isNearLadder)
				{
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb"))
					{
						rpgCharacterController.gravityTemp = rpgCharacterController.gravity;
						rpgCharacterController.gravity = 0;
						rpgCharacterController.rb.useGravity = false;
						rpgCharacterController.animator.applyRootMotion = true;
						rpgCharacterController.animator.SetTrigger("Climb-On-BottomTrigger");
						transform.position = new Vector3(rpgCharacterController.ladder.transform.position.x + .71f, transform.position.y, rpgCharacterController.ladder.transform.position.z);
						Vector3 relativePos = transform.position - rpgCharacterController.ladder.transform.position;
						Quaternion rotation = Quaternion.Euler(transform.rotation.x, rpgCharacterController.ladder.transform.rotation.y - 90f, transform.rotation.z);
						transform.rotation = rotation;
						rpgCharacterController.canMove = false;
						Invoke("Climbing", 1.05f);
					}
				}
				if(rpgCharacterController.rpgCharacterState == RPGCharacterState.CLIMBING)
				{
					if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top"))
					{
						rpgCharacterController.animator.applyRootMotion = true;
						rpgCharacterController.animator.SetTrigger("Climb-Off-TopTrigger");
						Invoke("EndClimbing", 2.6f);
					}
					if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up"))
					{
						rpgCharacterController.animator.applyRootMotion = true;
						rpgCharacterController.animator.SetTrigger("Climb-UpTrigger");
					}
					if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down"))
					{
						rpgCharacterController.animator.applyRootMotion = true;
						rpgCharacterController.animator.SetTrigger("Climb-DownTrigger");
					}
				}
			}
			//climbing while relaxed
			if(!blockGui && !rpgCharacterController.isBlocking && rpgCharacterController.isGrounded && rpgCharacterController.rpgCharacterState != RPGCharacterState.CLIMBING && rpgCharacterController.isNearLadder)
			{
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb"))
				{
					rpgCharacterController.gravityTemp = rpgCharacterController.gravity;
					rpgCharacterController.gravity = 0;
					rpgCharacterController.rb.useGravity = false;
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetTrigger("Climb-On-BottomTrigger");
					Invoke("Climbing", 1.05f);
				}
			}
			if(rpgCharacterController.rpgCharacterState == RPGCharacterState.CLIMBING)
			{
				if(GUI.Button(new Rect(30, 370, 100, 30), "Climb Off Top"))
				{
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetTrigger("Climb-Off-TopTrigger");
					Invoke("EndClimbing", 2.6f);
				}
				if(GUI.Button(new Rect(30, 410, 100, 30), "Climb Up"))
				{
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetTrigger("Climb-UpTrigger");
				}
				if(GUI.Button(new Rect(30, 445, 100, 30), "Climb Down"))
				{
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetTrigger("Climb-DownTrigger");
				}
			}
		}
		if(rpgCharacterController.isDead)
		{
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive"))
			{
				StartCoroutine(rpgCharacterController._Revive());
			}
		}
	}
}