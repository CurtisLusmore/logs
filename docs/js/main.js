(async function () {
  const groupLogs = function (logs, selector) {
    const groups = {};
    for (const log of logs) {
      const key = selector(log);
      if (!(key in groups)) groups[key] = { key, logs: [] };
      groups[key].logs.push(log);
    }
    return [...Object.values(groups)].sort((g1, g2) => g2.logs.length - g1.logs.length);
  }

  const addRows = function (table, rows, prefix='') {
    const body = table.getElementsByTagName('tbody')[0];
    for (const row of rows) {
      const tr = document.createElement('tr');
      tr.innerHTML = `<td><a href="${prefix}${row.key}">${row.key}</a></td><td>${row.logs.length.toLocaleString()}</td>`;
      body.appendChild(tr);
    }
  }

  const resp = await fetch('https://log.lusmo.re/api/get');
  const logs = (await resp.json()).sort((a, b) => b.datetime - a.datetime);
  const pages = groupLogs(logs, log => log.pathname);
  addRows(document.getElementById('pages'), pages);
  const domains = groupLogs(logs, log => log.referrer && new URL(log.referrer).hostname);
  addRows(document.getElementById('referrers'), domains, 'https://');
}());