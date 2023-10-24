# Helheim
Helheim(WT) is a top down 2d PvP focused action combat game

This is a WIP prototype developed by 1 programmer and 1 artist over 6 months.
The current demo scene was initially built for IndieCade, but was modified since then and has some features in experimental states (namely, the UI)

Early gameplay trailer: https://www.youtube.com/watch?v=2a_1QrNX0rU

Requirements:
- Unity Hub
- Xbox Controller (two to be played correctly)

Setup:
- Open the project using Unity hub to get the correct version of Unity
- Open Scene DemoTest2023
- Run the scene
- Press Start with your controller
- Press Back on your controller to emulate the start button on the other player's controller
- (The order that the players press start determines which team they'll be on and which hardcoded loadout they will control)  
- You can also press Enter to force both players to hit start, but you will need a gamepad to play

Controls:
- LStick: Movement
- RStick: Nothing
- RB: Light Attack
- RT: Heavy Attack
- RT (Hold): Charged Heavy Attack
- LB: Special Attack
- LT: Special Heavy Attack (Not Yet Implemented)
- A: Interact (Not Yet Impelmented)
- B: Dodge
- B (Hold): Sprint
- Tap B during Sprint: Jump
- X: Special Ability 1
- Y: Special Ability 2

Notes:
- Fireball can be charged
- Abilities require mana
- Mana is generated by landing attacks
- Shuriken have 3 rounds, they don't require ammo
  You reload them with mana by using the ability when you're out
- This version is in an in-between state where we were removing the HUD and replacing it with on character indicators.
  The on character indicators for health, mana, and shuriken ammo haven't been implemented yet.
  (There is an old over head HUD that can be enabled with some effort.)
