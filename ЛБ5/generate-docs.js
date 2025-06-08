const fs = require('fs');
const path = require('path');
const xml2js = require('xml2js');

const docsDir = './docs';
const xmlFiles = [
    'DatabaseService.xml',
    'DatabaseController.xml',
    'Program.xml',
    'Animal.xml',
    'Customer.xml',
    'Sale.xml',
];

if (!fs.existsSync(docsDir)){
    fs.mkdirSync(docsDir);
}

xmlFiles.forEach(xmlFile => {
    const xmlPath = path.join(docsDir, 'xml', xmlFile);
    const xmlData = fs.readFileSync(xmlPath, 'utf8');
    
    xml2js.parseString(xmlData, (err, result) => {
        if (err) throw err;
        
        const className = result.doc.members[0].member[0].$.name.split(':')[1];
        const classSummary = result.doc.members[0].member[0].summary[0];
        const classRemarks = result.doc.members[0].member[0].remarks ? result.doc.members[0].member[0].remarks[0] : '';
        
        let methodsHtml = '';
        result.doc.members[0].member.slice(1).forEach(member => {
            if (member.$.name.startsWith('M:')) {
                methodsHtml += generateMethodHtml(member);
            }
        });
        
        const htmlContent = fs.readFileSync(path.join(docsDir, 'template.html'), 'utf8')
            .replace('${CLASS_NAME}', className)
            .replace('${CLASS_SUMMARY}', classSummary)
            .replace('${CLASS_REMARKS}', classRemarks)
            .replace('${METHODS_SECTION}', methodsHtml);
        
        fs.writeFileSync(path.join(docsDir, `${className}.html`), htmlContent);
    });
});

function generateMethodHtml(member) {
    const methodName = member.$.name.split(':')[1]
        .replace(/\(/g, '(<span class="params">')
        .replace(/\)/g, '</span>)')
        .replace(/,/g, ', ');

    const summary = member.summary?.[0] || '';
    const remarks = member.remarks?.[0] ? `
        <div class="remarks">
            <strong>Примечания:</strong>
            <p>${member.remarks[0]}</p>
        </div>
    ` : '';

    let paramsHtml = '';
    if (member.param) {
        paramsHtml = member.param.map(p => `
            <div class="param">
                <strong>${p.$.name}:</strong> ${p._}
            </div>
        `).join('');
    }

    const returnsHtml = member.returns?.[0] ? `
        <div class="returns">
            <strong>Возвращает:</strong> ${member.returns[0]}
        </div>
    ` : '';

    const exceptionsHtml = member.exception?.[0] ? `
        <div class="exceptions">
            <strong>Исключения:</strong>
            <ul>
                ${member.exception.map(e => `
                    <li><code>${e.$.cref}</code> - ${e._}</li>
                `).join('')}
            </ul>
        </div>
    ` : '';

    return `
    <div class="method">
        <div class="method-name">${methodName}</div>
        <div class="summary">${summary}</div>
        ${paramsHtml}
        ${returnsHtml}
        ${exceptionsHtml}
        ${remarks}
    </div>
    `;
}