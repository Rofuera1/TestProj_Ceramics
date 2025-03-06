# TestProj_Ceramics
## Overview
Needed to implement:
For any offset:
∀m ∈ Model: offset × m ∈ Space  
  
Therefore, we can take m(0) and s(i) ∈ Space, where:  
offset(i) × m(0) = s(i)   
Then offset(i) = m(0)^(-1) × s(i)   
  
Check it against ∀m ∈ Model, for each i from [0 to Space.length)
## Used plugins
- Zenject
- Addressables
## Code overview
Code is up to MVC pattern, so 
- `Scripts/Controllers/Controller.cs` - is the main entry
- `Scripts/Models` contains *Model* and *Space* - objects to hold information from Model ans Space, the JSON
- `Scripts/View` is simply for UI & Button Management (all implemented in *UIManager*)
## Input, Output
- Input is implemented with addressables - could've been done through StreamingAssets, but for showing the skills sake, chose Addressables.
- Output is easier: the result is loaded to `Application.persistentDataPath/Result.json`
## Commentary
The search could've been done much faster - now it's slowed down for the visual's sake   
Also, in the `Space.cs`, the `ApproximatelyEqual` function checks if two Matrix4x4 are equal to some extend - without it, the float precision was too unpredictable. Now, the epsilon (how inaccurate they can be) is set to 0.001f
