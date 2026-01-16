# UnityTool
- UIManagerTool ：快速管理ui组件
- unity小工具  
- 批量添加或移除碰撞体  
- 删除重复组件  
# UIManager使用方法
导入unitypackage之后会有三个文件，
分别是UIManager.cs, UIManagerHeleper.cs,UIManagerEditor.cs
将UIManager.cs, UIManagerHeleper.cs挂载到场景中将UI根节点拖入场景，
并将相应的ui物体Tag改为"UI"，或者自定义，点击收集UI元素，再点击生成UI属性代码，即可通过例如：`UIManger.Instance.image.SetActive(false);`的方式进行控制。

<img width="366" height="594" alt="image" src="https://github.com/user-attachments/assets/1ec44f86-f0f7-4429-b18c-15a3db6b5606" />

# !物体名称必须遵顼C#命名规范

