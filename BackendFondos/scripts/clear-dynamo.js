const AWS = require('aws-sdk');
const dynamo = new AWS.DynamoDB.DocumentClient();

// Lista de todas tus tablas
const tables = [
    { name: 'Clientes', keySchema: ['ClienteID'] },
    { name: 'Fondos', keySchema: ['FondoID'] },
    { name: 'Transacciones', keySchema: ['TransaccionID'] }
];

// Borra ítems en lotes de 25 (límite de DynamoDB)
async function batchDelete(tableName, items, keySchema) {
    const batches = [];
    for (let i = 0; i < items.length; i += 25) {
        const batch = items.slice(i, i + 25).map(item => {
            const key = {};
            keySchema.forEach(k => key[k] = item[k]);
            return { DeleteRequest: { Key: key } };
        });
        batches.push(batch);
    }

    for (const batch of batches) {
        const params = {
            RequestItems: {
                [tableName]: batch
            }
        };
        await dynamo.batchWrite(params).promise();
    }
}

async function clearTable({ name, keySchema }) {
    try {
        const scanParams = { TableName: name };
        const data = await dynamo.scan(scanParams).promise();

        if (!data.Items || data.Items.length === 0) {
            console.log(`ℹ Tabla ${name} ya está vacía`);
            return;
        }

        await batchDelete(name, data.Items, keySchema);
        console.log(`✔ Tabla ${name} limpiada (${data.Items.length} ítems eliminados)`);
    } catch (err) {
        console.error(`❌ Error al limpiar ${name}:`, err.message);
    }
}

(async() => {
    for (const table of tables) {
        await clearTable(table);
    }
})();