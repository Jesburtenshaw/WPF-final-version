// dllmain.h : Declaration of module class.

class CCDMShellExtModule : public ATL::CAtlDllModuleT< CCDMShellExtModule >
{
public :
	DECLARE_LIBID(LIBID_CDMShellExtLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CDMSHELLEXT, "{09614f9a-0000-4e87-89e9-873111e4597a}")
};

extern class CCDMShellExtModule _AtlModule;
