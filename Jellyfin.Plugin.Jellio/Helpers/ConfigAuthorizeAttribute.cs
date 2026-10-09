using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.JellioDirect.Helpers;

public sealed class ConfigAuthorizeAttribute() : TypeFilterAttribute(typeof(ConfigAuthFilter));
