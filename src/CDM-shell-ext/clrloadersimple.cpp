#include "pch.h"
#include "clrloadersimple.h"
#include "tstring.h"
#include "pathinfo.h"


HRESULT CCLRLoaderSimple::CreateInstance(LPCWSTR szAssemblyName, LPCWSTR szClassName, IDispatch** ppvObject)
{
	HRESULT hr = E_FAIL;
	*ppvObject = nullptr;

	hr = LoadCLR();
	if (FAILED(hr))
	{
		return hr;
	}

	//Start the CLR
	hr = spRuntimeHost->Start();
	if (FAILED(hr))
		return hr;

	CComPtr<IUnknown> pUnk;

	hr = CreateAppDomain();

	CComPtr<_ObjectHandle> spObjectHandle;
	//Creates an instance of the type specified in the Assembly
	hr = spDefAppDomain->CreateInstance(
		_bstr_t(szAssemblyName),
		_bstr_t(szClassName),
		&spObjectHandle);

	if (FAILED(hr))
	{
		return hr;
	}

	CComVariant VntUnwrapped;
	hr = spObjectHandle->Unwrap(&VntUnwrapped);

	if (FAILED(hr))
		return hr;

	*ppvObject = VntUnwrapped.pdispVal;

	if (*ppvObject == nullptr)
		return E_FAIL;

	return S_OK;
}

HRESULT CCLRLoaderSimple::LoadCLR()
{
	HRESULT hr = S_OK;

	if (spRuntimeHost != nullptr)
		return hr;

	hr = BindToRuntimeV4();
	
	if (SUCCEEDED(hr) && (spRuntimeHost != nullptr))
	{
		hr = spRuntimeHost->Start();
	}

	return hr;
}

struct HMODULEHolder
{
	HMODULE _hm;
	HMODULEHolder(HMODULE hm)
		:_hm(hm)
	{
	}
	~HMODULEHolder()
	{
		FreeLibrary(_hm);
	}
};

HRESULT CCLRLoaderSimple::BindToRuntimeV4()
{
	CComPtr<ICLRMetaHost> spMetaHost;
	CComPtr<ICLRRuntimeInfo> spRuntimeInfo;

	HMODULE hm = LoadLibrary(L"mscoree.dll");
	if (hm == NULL)
	{
		return E_FAIL;
	}

	HMODULEHolder hh(hm);

	typedef HRESULT(__stdcall* CLRCreateInstancePtrType)(REFCLSID clsid, REFIID riid, /*iid_is(riid)*/ LPVOID* ppInterface);
	CLRCreateInstancePtrType CLRCreateInstancePtr = (CLRCreateInstancePtrType)GetProcAddress(hm, "CLRCreateInstance");
	if (CLRCreateInstancePtr == NULL)
	{
		return E_FAIL;
	}

	HRESULT hr = CLRCreateInstancePtr(CLSID_CLRMetaHost, IID_ICLRMetaHost, (void**)&spMetaHost);
	if (FAILED(hr))
	{
		return hr;
	}

	hr = spMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (void**)&spRuntimeInfo);
	if (FAILED(hr))
	{
		return hr;
	}

	hr = spRuntimeInfo->GetInterface(CLSID_CorRuntimeHost, IID_ICorRuntimeHost, (void**)&spRuntimeHost);
	if (FAILED(hr))
	{
		return hr;
	}

	return hr;
}

HRESULT CCLRLoaderSimple::CreateAppDomain()
{
	USES_CONVERSION;

	HRESULT hr = S_OK;

	if (spDefAppDomain.p != nullptr)
		return hr;
	
	CComPtr<IUnknown> pUnkDomainSetup;
	CComPtr<IAppDomainSetup> pDomainSetup;
	CComPtr<IUnknown> pUnkAppDomain;
	
	// Create an AppDomainSetup with the base directory pointing to the location of the managed DLL. We assume that the target assembly
	// is located in the same directory.
	hr = spRuntimeHost->CreateDomainSetup(&pUnkDomainSetup);
	if (FAILED(hr))
		return hr;

	hr = pUnkDomainSetup->QueryInterface(__uuidof(pDomainSetup), (LPVOID*)&pDomainSetup);
	if (FAILED(hr))
	{
		return hr;
	}

	// Get the location of the hosting shim DLL, and configure the AppDomain to search for assemblies in this location.
	tstring strDirectory = utilities::path_info::get_dll_folder(_AtlBaseModule.GetModuleInstance());

	pDomainSetup->put_ApplicationBase(CComBSTR(strDirectory.c_str()));

	// Create an AppDomain that will run the managed assembly, and get the
	// AppDomain's _AppDomain pointer from its IUnknown pointer.
	hr = spRuntimeHost->CreateDomainEx(strDirectory.c_str(), pUnkDomainSetup, 0, &pUnkAppDomain);
	if (FAILED(hr))
	{
		return hr;
	}

	return pUnkAppDomain->QueryInterface(__uuidof(spDefAppDomain), (LPVOID*)&spDefAppDomain);
}
