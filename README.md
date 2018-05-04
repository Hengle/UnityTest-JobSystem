### Testing Feature in Unity - JobSystem
TestinTesting New Feature: JobSystem in 2018.1

* Using Unity 2018.1.0f2

### Testing Scene
* There are 40000 cubes in the scene, all the cube collider are disable.
* Update all GameObject position every frame with sine function.

### Testing Result
![normal-update](https://raw.githubusercontent.com/douduck08/UnityTest-JobSystem/master/img/normal-update.jpg)
![use-job-system](https://raw.githubusercontent.com/douduck08/UnityTest-JobSystem/master/img/use-job-system.jpg)

[Demo in Youtube](https://www.youtube.com/watch?v=Tu-k_ZG1sOs)

* Without JobSystem, updating all GameObject in Update() cost 18 ms.
* With JobSystem, updating all GameObject in Jobs coat 7 ms.
* `IJobParallelForTransform` works better on root GameObject.

#### Ref
* C# Job System: https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/job_system.md
* C# Job System Cookbook: https://github.com/stella3d/job-system-cookbook
* Unity - Scripting API: IJob: https://docs.unity3d.com/2018.1/Documentation/ScriptReference/Unity.Jobs.IJob.html
* Unite Europe 2017 - C# job system & compiler: https://www.youtube.com/watch?v=AXUvnk7Jws4