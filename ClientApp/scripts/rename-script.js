const fs = require('fs');
const t = Date.now();

async function renameConent(file, find, replace) {
  return new Promise(async (resolve, reject) => {
    //var fs = require('fs')
    fs.readFile(file, 'utf8', async (err, data) => {
      if (err) {
        reject(err);
      }
      var regex = new RegExp(find, 'g');
      var result = data.replace(regex, replace);

      fs.writeFile(file, result, 'utf8', async (err) => {
        if (err) {
          reject(err);
        }
        resolve();
      });
    });
  });
}

(async function () {
  await renameConent(
    'dist/admgmt/index.html',
    '<base href="/">',
    `<script type="text/javascript">
  try {

    document.write("<base href='" + window.location.protocol + '//' + window.location.host + '/' + document.location.pathname.split('/')[1] + "/' >");
  } catch (e) {
    document.write("<base href='/' >");
  }
</script>`
  );
})();
