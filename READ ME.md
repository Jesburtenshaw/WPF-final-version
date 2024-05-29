STEPS

1. Build all projects from Solution
2. Include following DLLs in the same folders mentioned below.
CDM.dll 
CDMWrapper.dll 
Microsoft.Expression.Interactions.dll 
System.Windows.Interactivity.dll 
CDM-shell-ext.dll

src\_bin\output
&
src\CDM-shell-ext

3. Register extension 

4. if do not see option in Explorer. Restart Explorer from Task Manager


Register
--------
regsvr32 CDM-shell-ext.dll

Unregister
-----------
regsvr32 /u CDM-shell-ext.dll


Latest Path: ..\src\_bin\output\CDM-shell-ext.dll
