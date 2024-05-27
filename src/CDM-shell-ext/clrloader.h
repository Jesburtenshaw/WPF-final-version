////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
// FSDrive Shell Namespace Extension
//
// Copyright (c) 2021
//
// Company	: Tierfive
//
// File     : clrloader.h
//
// Contents	: Definition of the class to load .NET CLR
//
// Author	: Sergey Fasonov
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

#pragma once

#pragma comment(lib, "mscoree.lib")
#include "metahost.h"  // CLR 40 hosting interfaces

///
/// Class to load CLR and outlook assembly
///
class CCLRLoader
{
public:
	CCLRLoader();
	virtual ~CCLRLoader();

	HRESULT CreateInstance(LPCWSTR szAssemblyName, LPCWSTR szClassName, const IID &riid, void** ppvObject);
	HRESULT Unload();

private:
	HRESULT LoadCLR(LPCWSTR szAssemblyName);
	HRESULT CreateAppDomain(LPCWSTR szAssemblyName);
	HRESULT BindToCLR4OrAbove(LPCWSTR szAssemblyName);
	HRESULT FindLatestInstalledRuntime(ICLRMetaHost* pMetaHost, LPCWSTR wszMinVersion, ICLRRuntimeInfo** ppRuntimeInfo);
	void ParseClrVersion(LPCWSTR wszVersion, int rgiVersion[3]);
	// compare order of CLR versions represented as array of numbers
	BOOL IsClrVersionHigher(int rgiVersion[3], int rgiVersion2[3]);

	CComPtr<ICorRuntimeHost>		m_pRuntimeHost;
	CComPtr<mscorlib::_AppDomain>	m_pAppDomain;
};

