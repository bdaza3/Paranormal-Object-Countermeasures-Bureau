# Assignment 8 updates:
	Shaders:
To meet the requirement of incorporating at least one shader construct per programming team member, our group has integrated a range of shader-based visual effects that not only serve technical purposes but also enhance the horror atmosphere of our game environment:
Flashlight-Based Dynamic Lighting Shader
Our game environment is intentionally kept extremely dark to reinforce a sense of isolation and fear. Players can only navigate using a handheld flashlight, which is implemented through a dynamic lighting shader. This shader controls real-time illumination and ensures that only the areas directly within the flashlight’s beam are visible. By limiting visibility in this way, we direct the player's attention while also creating tension and suspense, hallmarks of effective horror game design.


Volumetric Fog Shader on the Second Floor
 To differentiate the second floor and elevate its mystery, we have implemented a light fog effect using a volumetric fog shader. This shader contributes to the visual storytelling by creating a mystic, dreamlike ambiance. The fog subtly alters the perception of distance and space, making navigation more uncertain and enhancing the eerie atmosphere without compromising performance.


Health-Based Post-Processing Shader (Screen Blood Effect)
 We use a screen-space post-processing shader to visualize player health. As the player takes damage from enemy encounters, a red overlay gradually intensifies, simulating a bleeding or dying effect. This shader dynamically responds to the player's health variable and culminates in a full red screen on game over. It provides immediate, immersive feedback on player status and reinforces the urgency and danger of the environment. It also decreases the intensity of the red overlay when the player stops getting attacked and gains health back slowly.
Two forms of writing
To make the game feel more immersive, we added a cutscene between the Menu scene and the game starting. Here we explain ‘vaguely’ what the game is about and the story behind it. The typing sound effect makes it sound like a story from horror movies, too. Then the game starts, and you are thrown into a dark, immersive environment.
The next form of writing is in the Credits scene. The player can access this scene from the Menu scene by pressing on the Credits button. We talk more about the game there, the authors, the references and more.
Alpha Testing Feedback:
These are the notes we took from people testing our game during the Alpha release:
More descriptive and informational directions in the objectives.
Sound indicators and attacks from the first floor monsters.
Hover text and responsiveness from items and door interactions.
Bigger interaction box collider in sliding doors for ease of interaction.
Simpler interaction with opening lockers and obtaining items.


	Beta Release changes from Alpha feedback:
We took the notes and ideas from the players/testers and made many changes to the game, as well as added a lot more features. 
Firstly, the game looked a bit too hard for the first-time players, who found it very difficult to ‘beat’ level 1 even after a few tries. So, to make the players feel a bit more confident in beating the game, we made the player a bit faster, the monsters a bit slower, and reduced the monsters’ range of attack.
Another problem we saw over and over again was the feeling of confusion among the testers. They seemed to have a hard time following directions and clues, and although the game is made like that on purpose, we tried to make it a bit easier by adding some more notes and objectives for players to know what they’re supposed to do and where to go. All classrooms have numbers, and notes are more concise. We also added a small feature that lets the player know when an object is obtainable (and with what key) when close.
New improvements and additions to the game:
We also added a lot more new features to the game this time around. The whole level 2 is with another type of monster with different abilities. The level 2 monster starts chasing you if you make a lot of noise when close, and this adds a new feeling of stealth to the game. When you need to open a vent by drilling, you better make sure the monster is far enough away from you so it can’t hear you, otherwise you better run.
Along with that, we also added 3 new obtainable objects: a lighter, a cloth, and a fuel can. These 3 objects are what the player needs to pick up in different rooms of the building, to actually ‘beat’ the game. A player only wins the game if they successfully avoid getting attacked by the monsters and also getting all 3 items into the Lab classroom.
Last but not least, we added a new feature to the game where now you can hide in lockers, and when the monster is chasing you, if you can’t outrun it because you ran out of stamina, you can now hide in the lockers and when the locker door is closed, you cannot get attacked. The monster also stops chasing you when you are inside a locker.
