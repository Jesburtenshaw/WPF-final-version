#pragma once

#pragma comment(lib, "mscoree.lib")
#include "metahost.h"  // CLR 40 hosting interfaces
#include <mscoree.h>
#include <atlbase.h>

#import <mscorlib.tlb> raw_interfaces_only high_property_prefixes("_get","_put","_putref") auto_rename

using namespace mscorlib;

///
/// Class to load CLR and outlook assembly
///
class CCLRLoaderSimple
{
public:
	HRESULT CreateInstance(LPCWSTR szAssemblyName, LPCWSTR szClassName, IDispatch** ppvObject);

private:
	HRESULT LoadCLR();
	HRESULT BindToRuntimeV4();
	HRESULT CreateAppDomain();

	CComPtr<ICorRuntimeHost> spRuntimeHost;
	CComPtr<_AppDomain> spDefAppDomain;
};

