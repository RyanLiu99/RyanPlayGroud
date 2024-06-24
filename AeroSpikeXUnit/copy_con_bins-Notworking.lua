function copy_con_bins(record)
    local pay_bins = {'key', 'PAYc', 'PAYs', 'PAYcc', 'PAYsc'}
    local con_bins = {'key', 'CONc', 'CONs', 'CONcc', 'CONsc'}

    local namespace = 'test'
    local pay_repo_set = 'PayRepo2'
    local con_repo_set = 'ConRepo2'

    local pay_record = map()
    local con_record = map()

    -- Extract the bins for PayRepo
    for _, bin in ipairs(pay_bins) do
        if record[bin] ~= nil then
            pay_record[bin] = record[bin]
        end
    end

    -- Extract the bins for ConRepo
    for _, bin in ipairs(con_bins) do
        if record[bin] ~= nil then
            con_record[bin] = record[bin]
        end
    end

    -- Write the new records to the respective sets
    if map.size(pay_record) > 0 then
        aerospike:create({ns = 'test', set = pay_repo_set, key = record['PK']}, pay_record)
    end
    if map.size(con_record) > 0 then
        aerospike:create({ns = namespace, set = con_repo_set, key = record['PK']}, con_record)
    end

    return 0
end
-- does not work. seems AE does not allow LUA work on 2 keys
-- cd /mnt/c/Ringba/Ringba-v2/Ringba.Infrastructure.Aerospike/LuaScripts
-- aql> register module 'copy_con_bins.lua'
-- aql> remove module 'copy_con_bins.lua'
-- aql> desc module 'copy_con_bins.lua'

-- aql> execute copy_con_bins.copy_con_bins() ON test.UsageRepo