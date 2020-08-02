(function () {
  if (navigator.doNotTrack === '1') return;
  const payload = {
    pathname: document.location.pathname,
    referrer: document.referrer
  };
  navigator.sendBeacon(
    'https://log.lusmo.re/api/log',
    JSON.stringify(payload)
  );
}());