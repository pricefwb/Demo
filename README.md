## Game Framework
Type.cs 需要添加 RuntimeAssemblyNames 和 RuntimeOrEditorAssemblyNames

"Game.Main",     //TODO: ADD My Runtime Assembly
"Game.Editor",   //TODO: ADD My Editor Assembly

Resource builder 中设置  GameBuildEventHandler

LitJson.dll
引入 LitJson.dll 并添加 LitJsonHelper.cs 
在base中设置 LitJsonHelper.cs 

目前分为 Mian,Hotfix 两个Assembly， 后续在做

## HybridCLR 按照官网教程
Hotfix.dll

ATO.Dll 泛型需要



## 继续完善。。。