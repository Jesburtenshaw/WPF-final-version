// cfsdriveshellext.cpp : Implementation of Cfsdriveshellext

#include "pch.h"
#include "cfsdriveshellext.h"
#include "fsdriveshellview.h"

CCfsDriveShellExt::CCfsDriveShellExt()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt constructor called");
}

CCfsDriveShellExt::~CCfsDriveShellExt()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt destructor called");
}

HRESULT CCfsDriveShellExt::FinalConstruct()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::FinalConstruct called");
	return S_OK;
}

void CCfsDriveShellExt::FinalRelease()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::FinalRelease called");
}

STDMETHODIMP CCfsDriveShellExt::GetClassID(__out CLSID* pclsid)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetClassID: Use CLSID_CfsDriveShellExt as main namespace class Id");
		*pclsid = CLSID_CfsDriveShellExt;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetClassID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::Initialize(PCIDLIST_ABSOLUTE pidl)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::Initialize: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::CreateViewObject(HWND hwndOwner, REFIID riid, __deref_out void** ppv)
{
	try
	{
		if (NULL == ppv)
			return E_POINTER;

		*ppv = NULL;

		LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::CreateViewObject: Try to get ViewObject for ShellFolder");
		if (riid == IID_IShellView)
		{
			_MainApplication->Initialize();

			LOGINFO(_MainApplication->GetLogger(), L"Create a new CShellViewImpl COM object");
			
			CComObject<CfsDriveShellView>* pShellView;
			if (!utilities::error_signaling::helper::check_hresult(CComObject<CfsDriveShellView>::CreateInstance(&pShellView)))
				return E_FAIL;

			CComPtr<IShellFolder> pFolder = this;
			pShellView->Init(pFolder);
			LOGINFO(_MainApplication->GetLogger(), L"Object initialization - pass the object its containing folder(this).");
			if (!utilities::error_signaling::helper::check_hresult(pShellView->QueryInterface(riid, ppv)))
				return E_FAIL;

			return S_OK;
		}
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::CreateViewObject: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetCurFolder(__out PIDLIST_ABSOLUTE* ppidl)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetCurFolder: Gets the current ShellFolder's pidl");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetCurFolder: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetDetailsOf(__in_opt PCUITEMID_CHILD pidl, UINT iColumn, __out SHELLDETAILS* psd)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDetailsOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::MapColumnToSCID(UINT iColumn, __out PROPERTYKEY* pkey)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::MapColumnToSCID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetDefaultColumn(DWORD /*dwRes*/, __out ULONG* plSort, __out ULONG* plDisplay)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDefaultColumn: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetDefaultColumnState(UINT iColumn, __out SHCOLSTATEF* pcsFlags)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDefaultColumnState: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::EnumObjects(HWND hwnd, SHCONTF grfFlags, __deref_out IEnumIDList** ppenmIDList)
{
	try
	{
		if (nullptr == ppenmIDList)
			return E_POINTER;

		*ppenmIDList = nullptr;

		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::EnumObjects: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetAttributesOf(UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, __inout SFGAOF* rgfInOut)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetAttributesOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::BindToObject(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::BindToObject: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::BindToStorage(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::BindToStorage: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::CompareIDs(LPARAM lParam, PCUIDLIST_RELATIVE pidl1, PCUIDLIST_RELATIVE pidl2)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::CompareIDs: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetDetailsEx(PCUITEMID_CHILD pidl, const PROPERTYKEY* pkey, __out VARIANT* pvar)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDetailsEx: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetUIObjectOf(HWND hwndOwner, UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, REFIID riid, __reserved UINT* /*rgfReserved*/, __deref_out void** ppv)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetUIObjectOf: Called - doesn't support for now");

		*ppv = nullptr;

		return E_NOINTERFACE;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetUIObjectOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::ParseDisplayName(_In_ HWND hwnd, _In_ IBindCtx* pbc, _In_ PWSTR pszDisplayName, __inout ULONG* pchEaten, __deref_out PIDLIST_RELATIVE* ppidl, __inout ULONG* pdwAttributes)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::ParseDisplayName: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::SetNameOf(HWND hwnd, PCUITEMID_CHILD pidl, LPCWSTR pszName, SHGDNF /*uFlags*/, __deref_out_opt PITEMID_CHILD* ppidlOut)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::SetNameOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::GetDefaultSearchGUID(__out GUID* /*pguid*/)
{
	try
	{
		LOGWARN(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDefaultSearchGUID: The plugin doesn't support this method");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDefaultSearchGUID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCfsDriveShellExt::EnumSearches(__deref_out IEnumExtraSearch** /*ppenum*/)
{
	try
	{
		LOGWARN(_MainApplication->GetLogger(), L"CCfsDriveShellExt::EnumSearches: The plugin doesn't support this method");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::EnumSearches: The exception occured!");
		return E_FAIL;
	}
}

const CCfsDriveShellExt::DISPLAYNAMEOFINFO CCfsDriveShellExt::_DisplayNameOfInfo[] =
{
	{ &CCfsDriveShellExt::_GetDisplayNameOfDisplayName },
	{ &CCfsDriveShellExt::_GetDisplayNameOfDisplayPath },
	{ &CCfsDriveShellExt::_GetDisplayNameOfParsingName },
	{ &CCfsDriveShellExt::_GetDisplayNameOfParsingPath },
};

STDMETHODIMP CCfsDriveShellExt::GetDisplayNameOf(PCUITEMID_CHILD pidl, SHGDNF uFlags, __out STRRET* psrName)
{
	try
	{
		HRESULT hr = S_OK;
		LOGINFO(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDisplayNameOf: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

		static const BYTE _indices[] =
		{
			//  FOREDITING  FORPARSING  FORADDRESSBAR  INFOLDER
			/*       0           0            0            0    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       0           0            0            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       0           0            1            0    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       0           0            1            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       0           1            0            0    */  DISPLAYNAMEOFINFO::GDNI_ABSOLUTEPARSING,
			/*       0           1            0            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEPARSING,
			/*       0           1            1            0    */  DISPLAYNAMEOFINFO::GDNI_ABSOLUTEFRIENDLY,
			/*       0           1            1            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       1           0            0            0    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       1           0            0            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       1           0            1            0    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       1           0            1            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY,
			/*       1           1            0            0    */  DISPLAYNAMEOFINFO::GDNI_ABSOLUTEPARSING,
			/*       1           1            0            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEPARSING,
			/*       1           1            1            0    */  DISPLAYNAMEOFINFO::GDNI_ABSOLUTEFRIENDLY,
			/*       1           1            1            1    */  DISPLAYNAMEOFINFO::GDNI_RELATIVEFRIENDLY
		};

		DWORD index = 0;

		if (uFlags & SHGDN_INFOLDER)
		{
			index |= DISPLAYNAMEOFINFO::GDNM_INFOLDER;
		}

		if (uFlags & SHGDN_FORPARSING)
		{
			index |= DISPLAYNAMEOFINFO::GDNM_FORPARSING;
		}

		if (uFlags & SHGDN_FORADDRESSBAR)
		{
			index |= DISPLAYNAMEOFINFO::GDNM_FORADDRESSBAR;
		}

		if (uFlags & SHGDN_FOREDITING)
		{
			index |= DISPLAYNAMEOFINFO::GDNM_FOREDITING;
		}

		hr = utilities::error_signaling::helper::check_hresult_retval((this->*_DisplayNameOfInfo[_indices[index]]._GetDisplayNameOf)(pidl, uFlags, &psrName->pOleStr));
		psrName->uType = STRRET_WSTR;

		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCfsDriveShellExt::GetDisplayNameOf: The exception occured!");
		return E_FAIL;
	}
}

HRESULT CCfsDriveShellExt::_GetDisplayNameOfDisplayName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszName))
		hr = S_FALSE;

	return hr;
}

HRESULT CCfsDriveShellExt::_GetDisplayNameOfDisplayPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszPath))
		hr = S_FALSE;

	return hr;
}

HRESULT CCfsDriveShellExt::_GetDisplayNameOfParsingName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszName))
		hr = S_FALSE;

	return hr;
}

HRESULT CCfsDriveShellExt::_GetDisplayNameOfParsingPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszPath))
		hr = S_FALSE;

	return hr;
}

