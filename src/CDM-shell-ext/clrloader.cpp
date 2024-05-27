
#include "pch.h"

#include "clrloader.h"
#include "application.h"

using namespace mscorlib;

CCLRLoader::CCLRLoader()
	: m_pRuntimeHost(nullptr),
	m_pAppDomain(nullptr)
{
}


CCLRLoader::~CCLRLoader()
{
	m_pAppDomain = nullptr;
	m_pRuntimeHost = nullptr;
}

HRESULT CCLRLoader::CreateInstance(LPCWSTR szAssemblyName, LPCWSTR szClassName, const IID &riid, void** ppvObject)
{
	HRESULT hr = E_FAIL;

	CComPtr<_ObjectHandle>	pObjectHandle;
	CComVariant             vDisp;

	LOGINFO(_MainApplication->GetLogger(), L"Load the CLR, and create an AppDomain for the target assembly.");
	if (FAILED(hr = LoadCLR(szAssemblyName)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"LoadCLR: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	LOGINFO(_MainApplication->GetLogger(), L"Create an AppDomain that will run the managed assembly.");
	if (FAILED(hr = CreateAppDomain(szAssemblyName)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"CreateAppDomain: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	// Create the managed aggregator in the target AppDomain, and unwrap it.
	// This component needs to be in a location where fusion will find it, ie
	// either in the GAC or in the same folder as the shim and the add-in.
	LOGINFO(_MainApplication->GetLogger(), L"Create the managed aggregator in the target AppDomain, and unwrap it");

	if (FAILED(hr = m_pAppDomain->CreateInstance(CComBSTR(szAssemblyName), CComBSTR(szClassName), &pObjectHandle)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"m_pAppDomain->CreateInstance: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	VariantInit(&vDisp);
	pObjectHandle->Unwrap(&vDisp);
	return vDisp.pdispVal->QueryInterface(riid, ppvObject);
}

HRESULT CCLRLoader::Unload()
{
	HRESULT hr = S_OK;
	CComPtr<IUnknown> pUnkDomain = m_pAppDomain;

	LOGINFO(_MainApplication->GetLogger(), L"UnLoad the CLR.");

	if (pUnkDomain.p != nullptr)
		hr = m_pRuntimeHost->UnloadDomain(pUnkDomain);
	
	m_pAppDomain = NULL;
	return hr;
}

HRESULT CCLRLoader::BindToCLR4OrAbove(LPCWSTR szAssemblyName)
{
	USES_CONVERSION;
	HRESULT hr = S_OK;
	
	CComPtr<ICLRMetaHost> pMetaHost;
	CComPtr<ICLRRuntimeInfo> pRuntimeInfo;
	
	WCHAR rgwchVersion[30];
	DWORD cwchVersion = ARRAYSIZE(rgwchVersion);

	LOGINFO(_MainApplication->GetLogger(), L"BindToCLR4OrAbove called");

	LOGINFO(_MainApplication->GetLogger(), L"Creates an instance of CLR MetaHost");

	if (FAILED(hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (void**)&pMetaHost)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"CLRCreateInstance: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	// Get the location of the hosting shim DLL, and retrieve the required 
	LOGINFO(_MainApplication->GetLogger(), L"Get the location of the hosting shim DLL");

	tstring strLibraryFileName = utilities::path_info::get_dll_folder(_AtlBaseModule.GetModuleInstance());
	strLibraryFileName = utilities::path_info::combine_path(strLibraryFileName, szAssemblyName);
	strLibraryFileName = utilities::path_info::combine_path(strLibraryFileName, L".dll");

	LOGINFO(_MainApplication->GetLogger(), L"The location is %s", strLibraryFileName.c_str());

	LOGINFO(_MainApplication->GetLogger(), L"Get the CLR version from the assembly file");
	
	if (FAILED(hr = pMetaHost->GetVersionFromFile(strLibraryFileName.c_str(), rgwchVersion, &cwchVersion)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"GetVersionFromFile: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	LOGINFO(_MainApplication->GetLogger(), L"The CLR version is %s", rgwchVersion);

	LOGINFO(_MainApplication->GetLogger(), L"Try binding to the same version of CLR the add-in is built against");
	// First try binding to the same version of CLR the add-in is built against
	if (FAILED(hr = pMetaHost->GetRuntime(rgwchVersion, IID_ICLRRuntimeInfo, (void**)&pRuntimeInfo)))
	{
		pRuntimeInfo.Release();
		
		LOGINFO(_MainApplication->GetLogger(), L"Gets CLR runtime version from its metadata");
		
		if (FAILED(hr = FindLatestInstalledRuntime(pMetaHost, rgwchVersion, &pRuntimeInfo)))
		{
			LOGERROR(_MainApplication->GetLogger(), L"FindLatestInstalledRuntime: HRESULT 0x%08X is failed", hr);
			return hr;
		}
	}

	LOGINFO(_MainApplication->GetLogger(), L"Ignores the result of SetDefaultStartupFlags - this is not critical operation");
	pRuntimeInfo->SetDefaultStartupFlags(STARTUP_LOADER_OPTIMIZATION_MULTI_DOMAIN_HOST, NULL);

	LOGINFO(_MainApplication->GetLogger(), L"Gets the ICorRuntimeHost interface");
	return pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost, IID_ICorRuntimeHost, (void**)&m_pRuntimeHost);
}

HRESULT CCLRLoader::LoadCLR(LPCWSTR szAssemblyName)
{
	HRESULT hr = S_OK;

	LOGINFO(_MainApplication->GetLogger(), L"Ensure the CLR is only loaded once.");
	if (m_pRuntimeHost != nullptr)
		return hr;
	
	LOGINFO(_MainApplication->GetLogger(), L"Binds to CLR");
	if (SUCCEEDED(hr = BindToCLR4OrAbove(szAssemblyName)) && (m_pRuntimeHost.p != nullptr))
	{
		LOGINFO(_MainApplication->GetLogger(), L"Start the CLR.");
		hr = m_pRuntimeHost->Start();
	}

	LOGINFO(_MainApplication->GetLogger(), L"The CLR is started");
	return hr;
}

HRESULT CCLRLoader::CreateAppDomain(LPCWSTR szAssemblyName)
{
	USES_CONVERSION;

	HRESULT hr = S_OK;

	LOGINFO(_MainApplication->GetLogger(), L"Ensure the AppDomain is created only once.");
	if (m_pAppDomain.p != nullptr)
		return hr;
	
	CComPtr<IUnknown> pUnkDomainSetup;
	CComPtr<IAppDomainSetup> pDomainSetup;
	CComPtr<IUnknown> pUnkAppDomain;
	CComBSTR cbstrAssemblyConfigPath;
	
	// Create an AppDomainSetup with the base directory pointing to the location of the managed DLL. We assume that the target assembly
	// is located in the same directory.
	LOGINFO(_MainApplication->GetLogger(), L"Create an AppDomainSetup with the base directory pointing to the location of the managed DLL.");
	if (FAILED(hr = m_pRuntimeHost->CreateDomainSetup(&pUnkDomainSetup)))
		return hr;

	LOGINFO(_MainApplication->GetLogger(), L"Queries the IAppDomainSetup");
	if (FAILED(hr = pUnkDomainSetup->QueryInterface(__uuidof(pDomainSetup), (LPVOID*)&pDomainSetup)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"pUnkDomainSetup->QueryInterface: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	// Get the location of the hosting shim DLL, and configure the AppDomain to search for assemblies in this location.
	LOGINFO(_MainApplication->GetLogger(), L"Get the location of the hosting shim DLL");

	tstring strDirectory = utilities::path_info::get_dll_folder(_AtlBaseModule.GetModuleInstance());

	LOGINFO(_MainApplication->GetLogger(), L"The location is %s", strDirectory.c_str());

	pDomainSetup->put_ApplicationBase(CComBSTR(strDirectory.c_str()));
	pDomainSetup->put_ApplicationName(CComBSTR(L"fsdrive.plugin"));

	tstring szConfigFile = strDirectory.c_str();
	szConfigFile += szAssemblyName;
	szConfigFile += L".dll.config";

	pDomainSetup->put_ConfigurationFile(CComBSTR(szConfigFile.c_str()));
	// Create an AppDomain that will run the managed assembly, and get the
	// AppDomain's _AppDomain pointer from its IUnknown pointer.
	LOGINFO(_MainApplication->GetLogger(), L"Create an AppDomain that will run the managed assembly");
	if (FAILED(hr = m_pRuntimeHost->CreateDomainEx(strDirectory.c_str(), pUnkDomainSetup, 0, &pUnkAppDomain)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"m_pRuntimeHost->CreateDomainEx: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	LOGINFO(_MainApplication->GetLogger(), L"Queries the IAppDomain");
	return pUnkAppDomain->QueryInterface(__uuidof(m_pAppDomain), (LPVOID*)&m_pAppDomain);
}

HRESULT CCLRLoader::FindLatestInstalledRuntime(ICLRMetaHost* pMetaHost, LPCWSTR wszMinVersion, ICLRRuntimeInfo** ppRuntimeInfo)
{
	USES_CONVERSION;

	CComPtr<IEnumUnknown> pEnum;
	CComPtr<ICLRRuntimeInfo> pRuntimeInfo, pLatestRuntimeInfo;

	ULONG cFetched;
	WCHAR rgwchVersion[30];
	DWORD cwchVersion;
	int rgiMinVersion[3]; //Major.Minor.Build
	int rgiVersion[3]; // Major.Minor.Build
	HRESULT hr = S_OK;

	*ppRuntimeInfo = NULL;

	LOGINFO(_MainApplication->GetLogger(), L"Find latest Runtime installed");

	// convert vN.N.N into an array of numbers
	LOGINFO(_MainApplication->GetLogger(), L"Parses the CLR version : %s", wszMinVersion);

	ParseClrVersion(wszMinVersion, rgiMinVersion);

	LOGINFO(_MainApplication->GetLogger(), L"Enumerates the runtimes");

	if (FAILED(hr = pMetaHost->EnumerateInstalledRuntimes(&pEnum)))
	{
		LOGERROR(_MainApplication->GetLogger(), L"pMetaHost->EnumerateInstalledRuntimes: HRESULT 0x%08X is failed", hr);
		return hr;
	}

	while (true)
	{
		pRuntimeInfo.Release();
		if (FAILED(hr = pEnum->Next(1, (IUnknown**)&pRuntimeInfo, &cFetched)))
			return hr;

		if (hr == S_FALSE)
			break;

		cwchVersion = ARRAYSIZE(rgwchVersion);

		if (FAILED(hr = pRuntimeInfo->GetVersionString(rgwchVersion, &cwchVersion)))
			return hr;

		LOGINFO(_MainApplication->GetLogger(), L"The runtime version is %s", rgwchVersion);

		ParseClrVersion(rgwchVersion, rgiVersion);
		if (IsClrVersionHigher(rgiVersion, rgiMinVersion) == FALSE)
			continue;

		rgiMinVersion[0] = rgiVersion[0];
		rgiMinVersion[1] = rgiVersion[1];
		rgiMinVersion[2] = rgiVersion[2];

		LOGINFO(_MainApplication->GetLogger(), L"Uses the runtime version - %s", rgwchVersion);

		pLatestRuntimeInfo.Attach(pRuntimeInfo.Detach());
	}

	if (pLatestRuntimeInfo == NULL)
		return E_FAIL;

	*ppRuntimeInfo = pLatestRuntimeInfo.Detach();
	return S_OK;
}

void CCLRLoader::ParseClrVersion(LPCWSTR wszVersion, int rgiVersion[3])
{
	rgiVersion[0] = rgiVersion[1] = rgiVersion[2] = 0;

	LPCWCH pwch = wszVersion;
	for (int i = 0; i < 3; i++)
	{
		// skip the firtst character - either 'v' or '.' and add the numbers
		for (pwch++; L'0' <= *pwch && *pwch <= L'9'; pwch++)
			rgiVersion[i] = rgiVersion[i] * 10 + *pwch - L'0';

		if (*pwch == 0)
			break;

		if (*pwch != L'.')
		{
			// the input is invalid - do not parse any further
			break;
		}
	}
}

// compare order of CLR versions represented as array of numbers
BOOL CCLRLoader::IsClrVersionHigher(int rgiVersion[3], int rgiVersion2[3])
{
	for (int i = 0; i < 3; i++)
	{
		if (rgiVersion[i] != rgiVersion2[i])
			return rgiVersion[i] > rgiVersion2[i];
	}

	return FALSE;
}
