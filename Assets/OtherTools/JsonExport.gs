

function onOpen() {
  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var menuEntries = [
    {name: "Export JSON for this sheet", functionName: "exportSheet"},
  ];
  ss.addMenu("Export JSON", menuEntries);
}

function exportSheet(e) {
  console.log("-- exportSheet");

  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var sheet = ss.getActiveSheet();
  var rowsData = getRowsData(sheet);
  var json = convertToJsonStr(rowsData);
  displayText(json);
}


function getRowsData(sheet) {
  console.log("-- getRowsData");

  var headersRange = sheet.getRange(1, 1, 1, sheet.getMaxColumns());
  var headers = headersRange.getValues()[0];
  
  var dataRange = sheet.getRange(2, 1, sheet.getMaxRows(), sheet.getMaxColumns());
  var objects = convertToObjectList(dataRange.getValues(), headers);

  return objects;
}

function convertToJsonStr(object) {
  var jsonString = JSON.stringify(object, 
    null,
    '\u{2000}'.repeat(2));

  return jsonString;
}


function convertToObjectList(data, keys) {
  console.log("-- convertToObjectList");
  const compositeFields = new Set(["biomes"]);

  var objects = [];

  for (var i = 0; i < data.length; ++i) {
    var object = {};
    var hasData = false;

    for (var j = 0; j < data[i].length; ++j) {
      var cellData = data[i][j];
      if (isCellEmpty(cellData))
        continue;

      if (compositeFields.has(keys[j])) {
        cellData = convertStringToObject(cellData);
      }

      object[keys[j]] = cellData;
      hasData = true;
    }
    if (hasData) {
      objects.push(object);
    }
  }

  return objects;
}

function isCellEmpty(cellData) {
  return typeof(cellData) == "string" && cellData == "";
}

function convertStringToObject(text) {
  var object = {};

  var lines = text.split("\n");
  for (var line of lines) {
    var tokens = line.split(":");
    var key = tokens[0];
    var value = parseFloat(tokens[1]);
    if (isNaN(value))
      value = tokens[1];

    object[key] = value;
  }

  return object;
}

function displayText(text) {
  console.log("-- displayText");

  var output = HtmlService.createHtmlOutput("<textarea style='width:100%;' rows='20'>" + text + "</textarea>");
  output.setWidth(400)
  output.setHeight(300);
  SpreadsheetApp.getUi().showModalDialog(output, 'Exported JSON');
}
