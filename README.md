# eMoji
</br>
Generative face animation and modelling Tool

</br>

</br>
---Key Controls---</br>
</br>
CAMERA :</br> 
TURN > Mousewheel click</br>
DRAG > Right Click</br>
ZOOM > Mousewheel</br>
</br>
</br>
## OVERVIEW ##</br>
</br>
the eMOJI Unity Asset is a project which allows you to animate face of your character, generate severals faces, </br>
generate Hair procedurally, with FBX Files or other 3D formats , but you can add face animation over FBX rigged animations.</br>
You can edit, save , or load during runtime baked animations , thought to be low- ressources process.</br>
the eMOJI Unity Asset can also be used for generic Vertex Anim over regular shapes ( and will use differents tools to edit the vertex position )</br>
He allows you also to edit the uv map for your character , change the skin color , select material , and perhaps in future generate cloth.</br>
We want also to generate humans with age parameter , including body proportions . For the character model , we want to notice that we</br>
used MakeHuman software , a great free software , which provide a good quality face indexing with a reasonable number of poly.</br>
Because of this good quality face indexing, we are just intent to change the vertices positions for the animations and the character creation.</br>
</br>
</br>

## TROUBLES ##</br>
</br>
FBX Transosing</br>
FBX index linking for your vertices is different usally than other 3D formats , as DAE for example.</br>
RayTracing over mesh collider from a .fbx file is inefficient in Unity , as I tried for my experience ( but I have to confirm or infirm it ... )</br>
So we have to transpose the mesh indexes from FBX to DAE , a process which complexify the code.</br>
</br>
Unity integration</br>
eMoji should be great as an external application to be exportable on each game engine , projects and more.</br>
We built eMoji in Unity in a first step because of all features specially raytracing , click to realWorldPos , or FBX rigged animations reading.</br>
Because we want to provide generative solutions with a reasonable cost for each machine , we will think about it in a future time , but it should required</br>
too heavy works to solve it quickly , because in this case we should create our own engine , including neceserally raytracing with mesh colliding and to</br>
build and load rigged animations from fbx files standards.</br>
Recently, I was also very interested to work more on Unreal Engine for my future projects , just to get better graphics results and quick programming with Blueprint, and </br>
this is why I really want to code this in an eportable appplication.</br>
</br>
Shared Mesh</br>
Because of the optimized fbx resource requiring , Unity use a sharedMesh model with FBX files,  stored in cache , so we cannot modifiy separately 2 characters depending on the same </br>
sharedMesh.</br>
</br>
Concurrence</br>
Obviously , we know they are so many talented programmers teams who already works on this idea of character generation , if I want to quote some of them I will quote Mixamo , Faceshift , </br>
Black desert and  also more ( but so talented ) not very well-known projects from 3D programmers </br>
(Youtube links)</br>
I want to quote SALSA also , which is a great asset providing generative facial animation with written text.</br>
This is absolutely the goal of this project , generative approach of character creation.</br>
To end the troubles section , I want to say that the further goal is not to build a high realistic characters ( because Unity graphics doesn't afford the best , expect if you're a talented shader programmer)</br>
but more to offer generative and exportable solutions for baked face animations , generative animation and generative creation of character ( with hairs and clothes )</br>
</br>
</br>
## RESEARCHES ##</br>
</br>
Hair shaders</br>
</br>
## PRINCIPLES ##</br>
</br>
</br>



### DESIGN YOUR FACE ANIMATION ###</br>
</br>
1 - Basically, you first put your models on eMoji and start tracking . </br>
	OR</br>
	You choose the model provided for the exampless , which contains already vertice groups </br>
	The vertices grid corresponding to your fbx file vertices positions ( after index transposing ) ,adjust the bouding box where you gonna load all vertices from the FBX file.</br>
2 - You select vertices you want to add to your vertice selection , navigating around your mesh.</br>
3 - When your vertice selection is done , you can save your selection as a file.</br>
4 - Then , you select the vertice selection and you can edit your vertice , for example if you selected the nose of your character you can turn it bigger , etc ...</br>
5 - When you're done with your nose editing , you save it in a keyframe . Go to an other keyframe , and edit an other state of your character nose. When you're done,</br>
	you just have to run the animation and save it when you want . You can edit keyframes, and vertices positions are interpolated in the animation.</br>
	Finally you just have to save the anim file and you're done. You will be able to load it in runtime of your game calling it from everywhere.</br>
	</br>
	</br>
### GENERATE A FACE ANIMATION IN RUNTIME ###</br>
</br>
### DESIGN YOUR CHARACTER ###</br>
1 - Basically, you first put your models on eMoji and start tracking . </br>
	OR</br>
	You choose the model provided for the exampless , which contains already vertice groups </br>
	The vertices grid corresponding to your fbx file vertices positions ( after index transposing ) ,adjust the bouding box where you gonna load all vertices from the FBX file.</br>
2 - You select vertices you want to add to your vertice selection , navigating around your mesh.</br>
3 - When your vertice selection is done , you can save your selection as a file.</br>
4 - Then , you select the vertice selection and you can edit your vertice , for example if you selected the nose of your character you can turn it bigger , etc ...</br>
5 - You can also design the hair of your character and your clothes.</br>
6 - When you're done , you just have to save your character in a file , and you can load it in runtime of your game calling it from everywhere.</br>
</br>
</br>
### GENERATE A CHARACTER IN RUNTIME ###</br>
</br>
</br>
### GENERATE HAIR IN RUNTIME ###</br>
</br>
</br>


## SCRIPTS ##</br>
</br>
</br>
---ANIMATION---</br>
</br>
	BakedAnimationController.cs > Permet de creer , editer et jouer des animations bakees , cest a dire des animations qui sont gerer en fonction de vertex groups</br>
	FaceMorpher.cs </br>
	RiggingController.cs</br>
	SkinnedFaceController.cs</br>
	TrackerAnimator.cs</br>
	</br>
---APPEARENCE---</br>
</br>
	BuildSkinColor.cs</br>
	ChangeSkinTexture.cs</br>
	</br>
---CAMERA---</br>
</br>
	DragCamera.cs</br>
	NavigateCamera.cs</br>
	ResetCameraPos.cs</br>
	RotateAround.cs</br>
	SwitchCameraClipping.cs</br>
	</br>
---DEBUG---</br>
</br>
	DebugMeshs.cs</br>
	RaytracingDebug.cs</br>
	</br>
---DISPLAY---</br>
</br>
	ChangeGridCubeSize.cs</br>
	HitpointScript.cs</br>
	</br>
---EYE---</br>
</br>
	ConstrainEyeRotation.cs</br>
	EyeFocusMover.cs</br>
	MoveEye.cs</br>
	MoveEyeGoal.cs</br>
	</br>
---FILE---</br>
</br>
	LoadAnimFile.cs</br>
	LoadFile.cs</br>
	LoadMeshFile.cs</br>
	ReadFaceFile.cs</br>
	SaveAnimFile.cs</br>
	SaveFaceFile.cs</br>
	SaveFile.cs</br>
	SaveMeshFile.cs</br>
	SkinnedLoadFile.cs</br>
	</br>
---GUI---</br>
</br>
	GUIOptions.cs</br>
	SkinnedGUIOptions.cs</br>
	</br>
---MESH---</br>
</br>
	FbxToMeshCollider.cs</br>
	LinkDaeToFbx.cs</br>
	</br>
---PROCEDURAL HAIR---</br>
</br>
	BuildHairGrid.cs</br>
	BuildHairPoints.cs</br>
	BuildPearls.cs</br>
	HairGenerator.cs</br>
	HairGenerator_1.cs</br>
	HairGenerator_Modulate.cs</br>
	HairGenerator_Old.cs</br>
	TerrainGenerator.cs</br>
	</br>
---TRACKING---</br>
</br>
	FaceTracking.cs</br>
	HumanGrid.cs</br>
	HumanGrid2.cs</br>
	SkinnedFaceTracing.cs</br>
	SkinnedFaceTracing_Old.cs</br>
	</br>
	</br>
	