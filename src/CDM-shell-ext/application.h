
#pragma once

#include "singleton.h"
#include "threading.h"
#include "log.h"
#include "clrloader.h"
#include "tierfiveshellext_i.h"

class CApplication
{
public:
	CApplication();
	~CApplication();

	// Initializes the application
	void Initialize();

	// Deinitialize the application
	void Done();

	// Returns the logger pointer
	utilities::diagnostics::CLogger<utilities::threading::CIntraProcessLock>* GetLogger() const { return m_pLogger.get(); }

	HWND CreateViewHostInstance(HWND hwndOwner);

private:
	auto_ptr<utilities::diagnostics::CLogger<utilities::threading::CIntraProcessLock>>	m_pLogger;
	auto_ptr<CCLRLoader>	m_pCLRLoader;

	CComPtr<IFSShellPlugin>	m_pShellPlugin;
	BOOL					m_bIsInitialized;

	// builds the log file name based on instance's folder and custom file name
	tstring BuildLogFileName();

	// Creates and initializes the Shell plugin
	void InitilaizeShellPlugin();
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
// CApplication singletone 

struct CApplicationSingleton : public utilities::Singleton<CApplication, CApplicationSingleton>
{
	CApplicationSingleton() : Singleton(1) {}
	static CApplication* init()
	{
		return new CApplication;
	}
};

extern struct CApplicationSingleton _MainApplication;

