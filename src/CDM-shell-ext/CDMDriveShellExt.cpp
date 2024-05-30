// cdmdriveshellext.cpp : Implementation of Cdmdriveshellext

#include "pch.h"
#include "CDMDriveShellExt.h"
#include "CDMDriveShellView.h"

CCDMDriveShellExt::CCDMDriveShellExt()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt constructor called");
}

CCDMDriveShellExt::~CCDMDriveShellExt()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt destructor called");
}

HRESULT CCDMDriveShellExt::FinalConstruct()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::FinalConstruct called");
	return S_OK;
}

void CCDMDriveShellExt::FinalRelease()
{
	LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::FinalRelease called");
}

STDMETHODIMP CCDMDriveShellExt::GetClassID(__out CLSID* pclsid)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetClassID: Use CLSID_CDMDriveShellExt as main namespace class Id");
		*pclsid = CLSID_CDMDriveShellExt;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetClassID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::Initialize(PCIDLIST_ABSOLUTE pidl)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::Initialize: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::CreateViewObject(HWND hwndOwner, REFIID riid, __deref_out void** ppv)
{
	try
	{
		if (NULL == ppv)
			return E_POINTER;

		*ppv = NULL;

		LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::CreateViewObject: Try to get ViewObject for ShellFolder");
		if (riid == IID_IShellView)
		{
			_MainApplication->Initialize();

			LOGINFO(_MainApplication->GetLogger(), L"Create a new CShellViewImpl COM object");
			
			CComObject<CDMDriveShellView>* pShellView;
			if (!utilities::error_signaling::helper::check_hresult(CComObject<CDMDriveShellView>::CreateInstance(&pShellView)))
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
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::CreateViewObject: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetCurFolder(__out PIDLIST_ABSOLUTE* ppidl)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetCurFolder: Gets the current ShellFolder's pidl");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetCurFolder: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetDetailsOf(__in_opt PCUITEMID_CHILD pidl, UINT iColumn, __out SHELLDETAILS* psd)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDetailsOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::MapColumnToSCID(UINT iColumn, __out PROPERTYKEY* pkey)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::MapColumnToSCID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetDefaultColumn(DWORD /*dwRes*/, __out ULONG* plSort, __out ULONG* plDisplay)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDefaultColumn: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetDefaultColumnState(UINT iColumn, __out SHCOLSTATEF* pcsFlags)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDefaultColumnState: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::EnumObjects(HWND hwnd, SHCONTF grfFlags, __deref_out IEnumIDList** ppenmIDList)
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
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::EnumObjects: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetAttributesOf(UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, __inout SFGAOF* rgfInOut)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetAttributesOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::BindToObject(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::BindToObject: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::BindToStorage(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv)
{
	try
	{
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::BindToStorage: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::CompareIDs(LPARAM lParam, PCUIDLIST_RELATIVE pidl1, PCUIDLIST_RELATIVE pidl2)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::CompareIDs: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetDetailsEx(PCUITEMID_CHILD pidl, const PROPERTYKEY* pkey, __out VARIANT* pvar)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDetailsEx: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetUIObjectOf(HWND hwndOwner, UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, REFIID riid, __reserved UINT* /*rgfReserved*/, __deref_out void** ppv)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetUIObjectOf: Called - doesn't support for now");

		*ppv = nullptr;

		return E_NOINTERFACE;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetUIObjectOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::ParseDisplayName(_In_ HWND hwnd, _In_ IBindCtx* pbc, _In_ PWSTR pszDisplayName, __inout ULONG* pchEaten, __deref_out PIDLIST_RELATIVE* ppidl, __inout ULONG* pdwAttributes)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::ParseDisplayName: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::SetNameOf(HWND hwnd, PCUITEMID_CHILD pidl, LPCWSTR pszName, SHGDNF /*uFlags*/, __deref_out_opt PITEMID_CHILD* ppidlOut)
{
	try
	{
		HRESULT hr = S_OK;
		return hr;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::SetNameOf: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::GetDefaultSearchGUID(__out GUID* /*pguid*/)
{
	try
	{
		LOGWARN(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDefaultSearchGUID: The plugin doesn't support this method");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDefaultSearchGUID: The exception occured!");
		return E_FAIL;
	}
}

STDMETHODIMP CCDMDriveShellExt::EnumSearches(__deref_out IEnumExtraSearch** /*ppenum*/)
{
	try
	{
		LOGWARN(_MainApplication->GetLogger(), L"CCDMDriveShellExt::EnumSearches: The plugin doesn't support this method");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::EnumSearches: The exception occured!");
		return E_FAIL;
	}
}

const CCDMDriveShellExt::DISPLAYNAMEOFINFO CCDMDriveShellExt::_DisplayNameOfInfo[] =
{
	{ &CCDMDriveShellExt::_GetDisplayNameOfDisplayName },
	{ &CCDMDriveShellExt::_GetDisplayNameOfDisplayPath },
	{ &CCDMDriveShellExt::_GetDisplayNameOfParsingName },
	{ &CCDMDriveShellExt::_GetDisplayNameOfParsingPath },
};

STDMETHODIMP CCDMDriveShellExt::GetDisplayNameOf(PCUITEMID_CHILD pidl, SHGDNF uFlags, __out STRRET* psrName)
{
	try
	{
		HRESULT hr = S_OK;
		LOGINFO(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDisplayNameOf: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

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
		LOGERROR(_MainApplication->GetLogger(), L"CCDMDriveShellExt::GetDisplayNameOf: The exception occured!");
		return E_FAIL;
	}
}

HRESULT CCDMDriveShellExt::_GetDisplayNameOfDisplayName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszName))
		hr = S_FALSE;

	return hr;
}

HRESULT CCDMDriveShellExt::_GetDisplayNameOfDisplayPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszPath))
		hr = S_FALSE;

	return hr;
}

HRESULT CCDMDriveShellExt::_GetDisplayNameOfParsingName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszName))
		hr = S_FALSE;

	return hr;
}

HRESULT CCDMDriveShellExt::_GetDisplayNameOfParsingPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath)
{
	HRESULT hr = S_OK;
	LOGINFO(_MainApplication->GetLogger(), L"CGDriveShlExt::_GetDisplayNameOfDisplayName: uFlags = %s", utilities::shell::helper::SHGDNF2String(uFlags).c_str());

	if (!utilities::shell::helper::CopyLPCWSTRToLPWSTR(L"Assetmax CRM", ppszPath))
		hr = S_FALSE;

	return hr;
}

