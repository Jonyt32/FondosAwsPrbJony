const AWS = require('aws-sdk');
const dynamo = new AWS.DynamoDB.DocumentClient();

const fondosIniciales = [
    { FondoID: 'F001', Nombre: 'Fondo Conservador', Riesgo: 'Bajo' },
    { FondoID: 'F002', Nombre: 'Fondo Balanceado', Riesgo: 'Medio' },
    { FondoID: 'F003', Nombre: 'Fondo Agresivo', Riesgo: 'Alto' }
];

async function insertarFondos() {
    const requests = fondosIniciales.map(item => ({
        PutRequest: { Item: item }
    }));

    const params = {
        RequestItems: {
            Fondos: requests
        }
    };

    try {
        await dynamo.batchWrite(params).promise();
        console.log(`✔ Fondos iniciales insertados (${fondosIniciales.length})`);
    } catch (err) {
        console.error(`❌ Error al insertar fondos:`, err.message);
    }
}

insertarFondos();