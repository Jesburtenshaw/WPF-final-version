STEPS

1. Build all projects from Solution
2. Include following DLLs in the same folders mentioned below.
CDM.dll 
CDMWrapper.dll 
Microsoft.Expression.Interactions.dll 
System.Windows.Interactivity.dll 
tierfive-shell-ext.dll

src\_bin\output
&
src\tierfive-shell-ext

3. Register extension 

4. if do not see option in Explorer. Restart Explorer from Task Manager


Register
--------
regsvr32 tierfive-shell-ext.dll

Unregister
-----------
regsvr32 /u tierfive-shell-ext.dll


Latest Path: ..\src\_bin\output\tierfive-shell-ext.dll
