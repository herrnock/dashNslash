RULEBOOK.md — ANCHOR POINT ACTION PROTOTYPE V0

VERSION
V0 Shield Defence Rewrite

PROJECT SUMMARY

Topdown Action Prototype built in true Unity 2D.

The player moves only by clicking on the playfield.
No WASD movement.

Movement is the combat system.

Before engaging, the player switches between:

- Attack Mode
- Defence Mode

The prototype exists to test whether movement-based combat is fun,
readable, controllable and worth continuing.


--------------------------------------------------
CORE DESIGN HYPOTHESIS
--------------------------------------------------

If movement is both offense and defense,
combat becomes a tense loop of positioning,
timing and mode selection.


--------------------------------------------------
PROTOTYPE QUESTIONS
--------------------------------------------------

1. Does anchor point click movement feel fun?

2. Can Attack and Defence both work through movement?

3. Are Attack and Defence clearly readable during play?

4. Does Shield Defence create meaningful defensive decisions?

5. Does Defence Dash create useful movement-based utility?


--------------------------------------------------
TECHNICAL DIRECTION
--------------------------------------------------

Use true Unity 2D.

- XY movement plane
- Orthographic camera
- SpriteRenderer
- Collider2D
- Rigidbody2D if needed

Do not use:

- 3D floor raycasts
- XZ movement
- NavMesh
- fake 2D in 3D space


--------------------------------------------------
PROJECT FOLDER STRUCTURE
--------------------------------------------------

Use the existing project structure exactly.

Assets/
├── 00_Scene
├── 01_Scripts
│   ├── 01_Gameplay
│   ├── 02_Core
│   ├── 03_Connectors
│   ├── 04_ConfigData
│   ├── 05_GameData
│   ├── 06_View
│   └── 07_Flow
├── 03_Prefabs
├── 04_Art
├── 05_Audio
├── 06_Docs


--------------------------------------------------
PROJECT SCOPE V0
--------------------------------------------------

IN SCOPE

- 1 small arena
- 1 player
- 1 enemy
- click-to-move movement
- Attack Mode
- Defence Mode
- shield charges
- shield absorb
- shield break
- optional shield recharge
- Defence Dash utility
- simple HP for player and enemy
- clear visual states
- win / lose / restart

OUT OF SCOPE

- no WASD movement
- no skills
- no items
- no progression
- no level select
- no multiple enemies
- no advanced AI
- no complex UI
- no camera systems
- no animation systems
- no content expansion


--------------------------------------------------
PLAYER MOVEMENT RULES
--------------------------------------------------

- Player moves only by left mouse click.
- Click converts screen position to world XY position.
- Click sets a movement target.
- New click immediately replaces old target.
- Player moves directly toward target.
- When target reached -> stop.
- While travelling -> Moving state.
- While stopped -> Idle state.

Movement must feel:

- responsive
- immediate
- controllable
- satisfying to repeat


--------------------------------------------------
MOVEMENT SYSTEM RULE
--------------------------------------------------

MovementState must NOT be a separate script.

MovementState is a nested enum inside:

Assets/01_Scripts/02_Core/MovementSystem.cs

Allowed states:

- Idle
- Moving


--------------------------------------------------
COMBAT MODE RULES
--------------------------------------------------

There are exactly two combat modes:

1. Attack Mode
2. Defence Mode

Player can switch between them at runtime.

Default input:

- Q = Attack Mode
- E = Defence Mode

Combat mode changes must be readable immediately.


--------------------------------------------------
ATTACK MODE RULES
--------------------------------------------------

Attack Mode is offensive.

If player contacts enemy while:

- in Attack Mode
- currently Moving

Then:

- enemy takes damage

Player is not protected while attacking.

Attack Mode is the high-risk aggressive mode.


--------------------------------------------------
DEFENCE MODE RULES
--------------------------------------------------

Defence Mode is defensive.

Defence Mode does NOT use directional front-arc blocking.

Defence Mode uses Shield Charges.

Player causes no direct damage while in Defence Mode.

If enemy contacts player while Shield Charges are available:

- 1 shield charge is consumed
- player takes no health damage from that absorbed hit
- shield absorb feedback should trigger

If enemy contacts player while no Shield Charges are available:

- player takes normal health damage

Defence Mode is not passive invulnerability.

Defence Mode is a resource-based defensive state.


--------------------------------------------------
REMOVED DEFENCE DESIGN
--------------------------------------------------

The old Directional Defence design is obsolete.

Do not reintroduce:

- front arc blocking
- movement direction defence checks
- incoming contact angle checks
- side contact failure rules
- rear contact failure rules
- DefenceArcView as required gameplay feedback

Defence is now based on ShieldSystem, not direction.


--------------------------------------------------
SHIELD SYSTEM RULES
--------------------------------------------------

ShieldSystem owns shield charge logic.

Default shield charges:

- 3 charges

ShieldSystem responsibilities:

- track current charges
- track max charges
- consume charge
- detect whether shield can absorb
- recharge shield
- support auto recharge
- support recharge on mode switch

When shield absorbs a hit:

- consume 1 charge
- prevent player health damage
- notify feedback systems if needed

When shield reaches 0:

- shield breaks
- player is forced into Attack Mode
- player is not stunned

Shield break must be readable.


--------------------------------------------------
SHIELD BREAK RULES
--------------------------------------------------

Shield break happens when the last remaining shield charge is consumed.

On shield break:

- force player into Attack Mode
- do not stun the player
- do not stop player movement automatically
- do not apply extra health damage for the absorbed hit
- trigger shield break feedback if available

Shield break creates vulnerability by removing Defence Mode,
not by disabling the player.


--------------------------------------------------
SHIELD RECHARGE RULES
--------------------------------------------------

Shield recharge is configurable.

Inspector-controlled options:

- autoRecharge: bool
- rechargeDelay: float
- rechargeOnModeSwitch: bool

Recharge can happen in two ways:

1. Automatic recharge

If autoRecharge is enabled:

- shield recharges after rechargeDelay
- exact timing is controlled by ShieldSystem
- recharge should not require player input

2. Recharge on mode switch

If rechargeOnModeSwitch is enabled:

- switching into Defence Mode can recharge shield
- this is triggered through the input / mode switching flow

Recharge behavior must remain easy to tune in Inspector.


--------------------------------------------------
DEFENCE DASH RULES
--------------------------------------------------

Defence Dash is movement-based utility.

If player contacts enemy while:

- in Defence Mode
- currently Moving

Then:

- enemy takes no direct damage
- optional knockback can be applied
- optional stun can be applied

Inspector-controlled options:

- applyKnockback: bool
- applyStun: bool
- enemyStunDuration: float

Defence Dash is not an attack.

Defence Dash exists to give Defence Mode active movement value.


--------------------------------------------------
ENEMY RULES V0
--------------------------------------------------

Enemy exists only to test movement combat.

Rules:

- enemy follows player
- enemy damages player on contact
- enemy has HP
- enemy dies at 0 HP
- enemy can optionally be stunned by Defence Dash
- enemy can optionally be knocked back by Defence Dash

Keep AI minimal.


--------------------------------------------------
ENEMY STUN RULES
--------------------------------------------------

EnemyStunSystem controls enemy stun state.

If enemy is stunned:

- enemy movement pauses
- enemy should resume movement after stun duration ends

Enemy stun is a temporary utility effect.

Enemy stun should not replace the main combat loop.


--------------------------------------------------
HEALTH RULES
--------------------------------------------------

Player has HP.
Enemy has HP.

Damage reduces HP.

If player HP <= 0:

-> Lose

If enemy HP <= 0:

-> Win

HealthSystem owns health values and death checks.

HealthSystem does not own:

- UI
- VFX
- SFX
- scene reload
- game flow transitions


--------------------------------------------------
CONTACT DAMAGE RULES
--------------------------------------------------

Enemy contact with player:

If player is in Defence Mode and shield can absorb:

- ShieldSystem consumes charge
- player takes no health damage

If shield cannot absorb:

- player takes normal contact damage

Player contact with enemy:

If player is in Attack Mode and Moving:

- enemy takes attack damage

If player is in Defence Mode and Moving:

- Defence Dash utility can trigger
- no direct enemy damage is dealt

Contact damage must use cooldowns where needed to avoid damage spam.


--------------------------------------------------
GAME FLOW
--------------------------------------------------

Target V0 flow:

Start
-> Combat
-> Win or Lose
-> Restart

Combat continues while:

- player is alive
- enemy is alive

Win condition:

- enemy HP <= 0

Lose condition:

- player HP <= 0

After Win or Lose:

- combat should stop
- damage should no longer be applied
- restart should be possible

Flow implementation belongs in:

Assets/01_Scripts/07_Flow


--------------------------------------------------
VISUAL READABILITY RULES
--------------------------------------------------

Attack Mode:

- use red / orange visuals
- successful hit must be obvious

Defence Mode:

- use blue visuals
- shield absorb must be obvious
- shield break must be obvious
- shield charge state should become readable when UI is added

Defence Dash:

- knockback should be visible
- stun should be visible or clearly inferable

General:

- readability > beauty
- primitive shapes allowed
- final art not needed
- feedback exists to support playtesting, not polish


--------------------------------------------------
CURRENT CORE SYSTEMS
--------------------------------------------------

Assets/01_Scripts/02_Core/

- MovementSystem
- CombatModeSystem
- HealthSystem
- EnemySystem
- EnemyContactDamageSystem
- PlayerAttackContactDamageSystem
- ShieldSystem
- EnemyStunSystem
- DefenceDashContactSystem


--------------------------------------------------
CURRENT CONNECTORS
--------------------------------------------------

Assets/01_Scripts/03_Connectors/

- PlayerInputConnector


--------------------------------------------------
CURRENT VIEW SYSTEMS
--------------------------------------------------

Assets/01_Scripts/06_View/

- TargetMarkerView
- CombatModeView
- HealthView
- HitFlashView
- BlockFeedbackView

BlockFeedbackView can be reused as Shield Absorb Feedback.


--------------------------------------------------
ARCHITECTURE MODEL
--------------------------------------------------

01_GAMEPLAY

Rules / enums / design meaning.

Examples:

- CombatMode
- shared gameplay definitions


02_CORE

Deterministic logic only.

Examples:

- MovementSystem
- CombatModeSystem
- HealthSystem
- EnemySystem
- ShieldSystem
- EnemyStunSystem
- contact damage systems

Core Systems must not contain:

- UI logic
- animation logic
- VFX logic
- SFX logic
- camera logic


03_CONNECTORS

Coordinate systems.

Examples:

- PlayerInputConnector
- future CombatResolver
- future EncounterController

Connectors may:

- read input
- call Core Systems
- coordinate mode switching
- coordinate feature-specific behavior


04_CONFIGDATA

Editable balancing values.

Examples:

- PlayerConfig
- EnemyConfig
- CombatConfig
- ShieldConfig if needed

Game Data contains values, not logic.


05_GAMEDATA

Runtime/session data if needed.


06_VIEW

Read state and visualize only.

Examples:

- PlayerView
- TargetMarkerView
- CombatModeView
- HealthView
- HitFlashView
- BlockFeedbackView

View may read gameplay state.

View must not change gameplay logic.


07_FLOW

Flow / states.

Examples:

- GameStateController
- RestartFlow
- WinLoseFlow

Flow controls gameplay-relevant sequence and end states.


--------------------------------------------------
ARCHITECTURE RULES
--------------------------------------------------

- Core Systems contain gameplay logic only.
- View reads state only.
- View must not change gameplay logic.
- Data contains values, not logic.
- Connectors coordinate systems.
- Flow controls game state and sequence.
- Keep V0 simple.
- No extra architecture unless needed.
- Do not create abstractions for future features unless the current task needs them.


--------------------------------------------------
COPILOT WORKFLOW RULE
--------------------------------------------------

Never give Copilot full project scope at once.

Implementation must happen task by task.

One task at a time.

After each task:

- verify result
- then continue with next task


--------------------------------------------------
CODE AGENT RULES
--------------------------------------------------

Do not silently add systems.

Do not create extra abstractions.

Do not rename folders.

Do not move unrelated files.

Use existing folder structure exactly.

Always report changed files.

Always verify scripts compile.

Always check Unity Console after changes.


--------------------------------------------------
SYSTEM INTERACTION CHECKPOINT
--------------------------------------------------

If a new system touches an existing system:

STOP.

Create a quick system sketch first.

Only continue after interaction is clear.


--------------------------------------------------
PROTOTYPE EVALUATION
--------------------------------------------------

Prototype phase only checks:

CLARITY

- Do I understand what to do?
- Do I understand Attack vs Defence?
- Do I understand when shield absorbs?
- Do I understand when shield breaks?

AGENCY

- Can better movement improve outcome?
- Can better mode switching improve outcome?
- Can I intentionally use Defence Dash?

MOTIVATION

- Do I want to retry?
- Do I want to switch modes?
- Do I want to experiment with movement timing?

Ignore for now:

- progression
- polish
- content depth
- final visuals
- monetization
- long-term build variety


--------------------------------------------------
FINAL CHECK AFTER EACH TASK
--------------------------------------------------

After each implementation task:

- verify scripts compile
- check syntax errors
- check Unity Console
- report created files
- report modified files
- report deleted or ignored files if relevant