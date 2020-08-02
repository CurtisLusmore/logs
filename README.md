# Logs

A DIY page view logger, built with [Azure Functions][af] and
[Azure Table Storage][ats], that records the pathname and referrer of all page
views on your website. This is ideal for static websites like blogs, and where
you are concerned about the privacy of more sophisticated tools like Google
Analytics.

Note that this is _not_ a fully-featured analytics suite that can track things
like bounce rates, session times, navigation flows, etc. If you need this kind
of information, you will need to either build it in yourself, or more likely,
use another service altogether.


## Dependencies

In order to deploy this you will need the following tools:
* [.NET Core CLI][dn]
* [Azure CLI][az]
* [Azure Functions Core Tools][ft]
* An Azure subscription


## Deployment

To deploy, simply run the `Deploy.ps1` file and supply the required values for
the names of the various resources as prompted.

```
$ .\Deploy.ps1
```


## Adding to website

Once you have deployed the infrastructure, add the following JavaScript snippet
to your website to log a page view.

```js
(function () {
  if (navigator.doNotTrack === '1') return;
  const payload = {
    pathname: document.location.pathname,
    referrer: document.referrer
  };
  navigator.sendBeacon(
    'https://your-deployment.azurewebsites.net/api/log',
    JSON.stringify(payload)
  );
}());
```

This snippet can also be found in `./js/log.js`.


## Custom Domain

To add a custom domain to the logging endpoint, follow the instructions on
[Map an existing Custom DNS name to Azure App Service][cd1] to register a
custom domain name, and then follow the instructions on
[Create a free certificate][cd2] to create and assign a certificate to enable
TLS.


[af]:  https://azure.microsoft.com/en-au/services/functions/
[ats]: https://azure.microsoft.com/en-us/services/storage/tables/
[dn]:  https://docs.microsoft.com/en-us/dotnet/core/tools/
[az]:  https://docs.microsoft.com/en-us/cli/azure/install-azure-cli/
[ft]:  https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local/
[cd1]: https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-custom-domain#get-domain-verification-id
[cd2]: https://docs.microsoft.com/en-us/azure/app-service/configure-ssl-certificate#create-a-free-certificate-preview
