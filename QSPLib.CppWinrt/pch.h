#pragma once

// https://stackoverflow.com/questions/1041866/what-is-the-effect-of-extern-c-in-c
extern "C" {
    #include "qsp/qsp.h"
    #include "qsp/bindings/default/qsp_default.h"
    #include "qsp/declarations.h"
    #include "qsp/actions.h"
    #include "qsp/callbacks.h"
    #include "qsp/coding.h"
    #include "qsp/codetools.h"
    #include "qsp/common.h"
    #include "qsp/errors.h"
    #include "qsp/game.h"
    #include "qsp/locations.h"
    #include "qsp/mathops.h"
    #include "qsp/menu.h"
    #include "qsp/objects.h"
    #include "qsp/statements.h"
    #include "qsp/text.h"
    #include "qsp/time.h"
    #include "qsp/variables.h"
    #include "qsp/playlist.h"
    #include "qsp/variant.h"
}

#include <unknwn.h>
#include <winrt/Windows.System.h>
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.UI.Xaml.h>
#include <winrt/Windows.UI.Xaml.Controls.h>
#include <winrt/Windows.UI.Xaml.Controls.Primitives.h>
#include <winrt/Windows.UI.Xaml.Data.h>
#include <winrt/Windows.UI.Xaml.Interop.h>
#include <winrt/Windows.UI.Xaml.Markup.h>
#include <winrt/Windows.UI.Xaml.Navigation.h>
#include <winrt/Windows.Storage.h>
#include <winrt/Windows.Storage.Streams.h>
#include <winrt/Windows.Security.Cryptography.h>

#include <chrono>
#include <iostream>
#include <sstream>
#include <locale>
#include <stringapiset.h>
#include <pplawait.h>

#include <fmt/core.h>
#include <fmt/format.h>