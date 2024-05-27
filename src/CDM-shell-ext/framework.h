#pragma once

#ifndef STRICT
#define STRICT
#endif

#include "targetver.h"

#define _ATL_APARTMENT_THREADED

#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit


#define ATL_NO_ASSERT_ON_DESTROY_NONEXISTENT_WINDOW

// Note: comdef.h will create only smart pointers for defined interfaces. Include all required
//       Win32 header files before including comdef.h
#include <comdef.h>

#include "resource.h"
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>

using namespace ATL;

// Win32 include files.
#include <shlobj.h>
#include <shobjidl.h>


#include "_atlcontrols.h"

// using std namespace
#include <iostream>
#include <vector>
#include <Windows.h>
#include <time.h>
#include <stdexcept>
#include <fstream>

using namespace std;

// using mscoree library
#import <mscorlib.tlb> raw_interfaces_only high_property_prefixes("_get","_put","_putref")

// For CorBindToRuntimeEx and ICorRuntimeHost.
#include <mscoree.h>


#include "tstring.h"
#include "singleton.h"
#include "threading.h"
#include "pathinfo.h"
#include "log.h"
#include "shellhelper.h"
#include "errorsignaling.h"