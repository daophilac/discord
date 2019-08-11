package com.peanut.discord.models;
import java.util.List;
public class Role {
    private int roleId;
    private int roleLevel;
    private boolean mainRole;
    private String roleName;
    private boolean kick;
    private boolean modifyChannel;
    private boolean modifyRole;
    private boolean changeUserRole;
    private int serverId;
    private Server server;
    private List<ChannelPermission> channelPermissions;
    public int getRoleId() {
        return roleId;
    }
    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }
    public int getRoleLevel() {
        return roleLevel;
    }
    public void setRoleLevel(int roleLevel) {
        this.roleLevel = roleLevel;
    }
    public boolean isMainRole() {
        return mainRole;
    }
    public void setMainRole(boolean mainRole) {
        this.mainRole = mainRole;
    }
    public String getRoleName() {
        return roleName;
    }
    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }
    public boolean isKick() {
        return kick;
    }
    public void setKick(boolean kick) {
        this.kick = kick;
    }
    public boolean isModifyChannel() {
        return modifyChannel;
    }
    public void setModifyChannel(boolean modifyChannel) {
        this.modifyChannel = modifyChannel;
    }
    public boolean isModifyRole() {
        return modifyRole;
    }
    public void setModifyRole(boolean modifyRole) {
        this.modifyRole = modifyRole;
    }
    public boolean isChangeUserRole() {
        return changeUserRole;
    }
    public void setChangeUserRole(boolean changeUserRole) {
        this.changeUserRole = changeUserRole;
    }
    public int getServerId() {
        return serverId;
    }
    public void setServerId(int serverId) {
        this.serverId = serverId;
    }
    public Server getServer() {
        return server;
    }
    public void setServer(Server server) {
        this.server = server;
    }
    public List<ChannelPermission> getChannelPermissions() {
        return channelPermissions;
    }
    public void setChannelPermissions(List<ChannelPermission> channelPermissions) {
        this.channelPermissions = channelPermissions;
    }
}
